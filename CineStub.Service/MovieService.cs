using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineStub.Data.Contracts;
using CineStub.Model;

namespace CineStub.Service
{
    public class MovieService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public IEnumerable<Movie> AllMovies
        {
            get { return Movies.GetAll(); }
        }

        public IEnumerable<Movie> GetCurrentMovies()
        {
            var maxDate = DateTime.Today.AddDays(7);
            
            DateTime startDate = DateTime.Today;

            while (startDate.DayOfWeek != DayOfWeek.Thursday)
            {
                startDate = startDate.AddDays(-1);
            }

            var schedule = Schedules
                .GetAllIncluding(new string[] {"Slots.Movie"})
                .OrderBy(s => s.StartDate)
                .FirstOrDefault(s => s.StartDate == startDate);

            if (schedule == null)
            {
                return new List<Movie>();
            }

            return GetMoviesForSchedule(schedule);
        }

        public Movie GetMovieDetails(int id)
        {
            return Movies.GetByIdIncluding(id, new string[] {"MovieCast.ActorIndices.Actor", "MovieImages"});
        }

        public IEnumerable<SlotGroup> GetMovieShowtimes(int id)
        {
            var maxDate = DateTime.Today.AddDays(7);

            var movieSlots =
                Slots.GetAllIncluding(s => s.Movie)
                    .Where(s => s.Movie.Id == id && s.IsRoot)
                    .Where(s => s.DateTime >= DateTime.Today)
                    .Where(s => s.DateTime < maxDate).ToList();

            var showtimeGroups = movieSlots
                .GroupBy(s => new DateTime(s.DateTime.Year, s.DateTime.Month, s.DateTime.Day))
                .Select(g => new SlotGroup {DateTime = g.Key, Slots = g.ToList()});

            return showtimeGroups;
        }

        public void AddMovieGraph(Movie movie)
        {
            var existingMovie = Movies.GetAll().FirstOrDefault(m => m.TmdbId == movie.TmdbId);
            
            if (existingMovie != null)
            {
                throw new DuplicateNameException(String.Format("{0} already exists.", movie.Title));
            }

            foreach (var person in Actors.GetAll())
            {
                foreach (var actorIndex in movie.MovieCast.ActorIndices)
                {
                    if (actorIndex.Actor == null) continue;
                    if (person.TmdbId != actorIndex.Actor.TmdbId) continue;
                    actorIndex.Actor = null;
                    actorIndex.ActorId = person.Id;
                }
            }

            Movies.Add(movie);
            UnitOfWork.SaveChanges();
        }




        private IEnumerable<Movie> GetMoviesForSchedule(Schedule schedule)
        {
            var movies = (from slot in schedule.Slots where slot.IsRoot select slot.Movie).ToList();

            return movies.GroupBy(m => m.Id).Select(g => g.First()).ToList();
        }



        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public IRepository<Movie> Movies
        {
            get { return _unitOfWork.Movies; }
        }

        public IRepository<Actor> Actors
        {
            get { return _unitOfWork.Actors; }
        }

        public IRepository<Schedule> Schedules
        {
            get { return _unitOfWork.Schedules; }
        }

        public IRepository<Slot> Slots
        {
            get { return _unitOfWork.Slots; }
        }
    }
}
