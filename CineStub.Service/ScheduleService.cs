using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using CineStub.Data.Contracts;
using CineStub.Model;

namespace CineStub.Service
{
    public class ScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Schedule> AllSchedules
        {
            get { return Schedules.GetAll(); }
        }

        public Schedule GetSchedule(int id)
        {
            return Schedules.GetById(id);
        }

        public Schedule AddSchedule()
        {
            var mostRecent = GetMostRecent();
            Schedule schedule;

            if (mostRecent != null)
            {
                var startDate = mostRecent.EndDate.AddDays(1);
                var endDate = startDate.AddDays(6);
                schedule = new Schedule(startDate, endDate);
            }
            else
            {
                var startDate = DateTime.Today;
                var endDate = DateTime.Today;
                while (endDate.DayOfWeek != DayOfWeek.Wednesday)
                {
                    endDate = endDate.AddDays(1);
                }

                schedule = new Schedule(startDate, endDate);
            }

            Schedules.Add(schedule);
            UnitOfWork.SaveChanges();

            return schedule;
        }

        public void ScheduleMovie(int requestedSlotId, int movieId)
        {
            var movie = Movies.GetById(movieId);
            var requestedSlot = Slots.GetById(requestedSlotId);

            if (movie == null)
            {
                throw new InstanceNotFoundException(String.Format("Movie with Id '{0}' not found.", movieId));
            }

            if (requestedSlot == null)
            {
                throw new InstanceNotFoundException(String.Format("Slot with Id '{0}' not found.", requestedSlotId));
            }

            var nRequiredSlots = CalculateRequiredSlots(requestedSlot, movie);

            var slots = GetOpenSlotsStartingWith(requestedSlot, nRequiredSlots).ToList();

            if (slots.Count() < nRequiredSlots)
            {
                throw new IndexOutOfRangeException(String.Format("{0} requires {1} slots.", movie.Title, nRequiredSlots));
            }

            foreach (var slot in slots)
            {
                slot.Movie = movie;
                slot.IsOpen = false;
            }

            requestedSlot.Movie = movie;
            requestedSlot.IsRoot = true;

            UnitOfWork.SaveChanges();
        }

        public void UnscheduleMovie(int rootSlotId)
        {
            var slot = Slots.GetAllIncluding(s => s.Movie).FirstOrDefault(s => s.Id == rootSlotId);
            
            if (slot == null)
            {
                throw new InstanceNotFoundException(String.Format("Slot with Id '{0}' not found.", rootSlotId));
            }
            if (slot.Movie == null)
            {
                throw new InstanceNotFoundException(String.Format("There is no movie at slot with id '{0}'", rootSlotId));
            }

            var movie = slot.Movie;
            var nMovieSlots = CalculateRequiredSlots(slot, movie);

            var slots = GetSlotsStartingWith(slot, nMovieSlots, true);

            foreach (var movieSlot in slots)
            {
                movieSlot.IsOpen = true;
                movieSlot.IsRoot = false;
                movieSlot.Movie = null;
            }

            UnitOfWork.SaveChanges();
        }

        public IEnumerable<SlotGroup> SlotsByDate(int scheduleId)
        {
            var schedule = Schedules.GetByIdIncluding(scheduleId, new string[] {"Slots.Movie"});

            var slots = schedule.Slots;

            var slotsByDate = slots
                .GroupBy(s => new DateTime(s.DateTime.Year, s.DateTime.Month, s.DateTime.Day))
                .Select(group => new SlotGroup {DateTime = group.Key, Slots = group.ToList()});

            return slotsByDate;
        }


        private Schedule GetMostRecent()
        {
            return Schedules.GetAll().OrderByDescending(s => s.EndDate).FirstOrDefault();
        }

        private IEnumerable<Slot> GetSlotsStartingWith(Slot firstSlot, int nSlots)
        {
            var query = Slots.GetAll()
                .Where(s =>
                    s.DateTime.Year >= firstSlot.DateTime.Year &&
                    s.DateTime.Month >= firstSlot.DateTime.Month &&
                    s.DateTime.Day >= firstSlot.DateTime.Day &&
                    s.DateTime.Hour >= firstSlot.DateTime.Hour)
                .OrderBy(s => s.DateTime)
                .ToList()
                .Take(nSlots);

            return query;
        }

        private IEnumerable<Slot> GetSlotsStartingWith(Slot firstSlot, int nSlots, bool onlySameDay)
        {
            var query = GetSlotsStartingWith(firstSlot, nSlots);

            if (onlySameDay)
            {
                return query
                    .Where(s =>
                        s.DateTime.Year == firstSlot.DateTime.Year &&
                        s.DateTime.Month == firstSlot.DateTime.Month &&
                        s.DateTime.Day == firstSlot.DateTime.Day);
            }

            return query;
        }

        private IEnumerable<Slot> GetOpenSlotsStartingWith(Slot firstSlot, int nSlots)
        {
            var query = GetSlotsStartingWith(firstSlot, nSlots, true);

            return query.TakeWhile(s => s.IsOpen);

            //if (onlySameDay)
            //{
            //    var query = Slots.GetAll()
            //        .Where(s =>
            //            s.DateTime.Year == firstSlot.DateTime.Year &&
            //            s.DateTime.Month == firstSlot.DateTime.Month &&
            //            s.DateTime.Day == firstSlot.DateTime.Day)
            //        .OrderBy(s => s.DateTime)
            //        .ToList()
            //        .SkipWhile(s => s.DateTime.Hour < firstSlot.DateTime.Hour)
            //        .Take(nSlots);

            //    return stopOnClosedSlot ? query.TakeWhile(s => s.IsOpen) : query;
            //}
            //else
            //{
            //    var query = Slots.GetAll()
            //        .Where(s =>
            //            s.DateTime.Year >= firstSlot.DateTime.Year &&
            //            s.DateTime.Month >= firstSlot.DateTime.Month &&
            //            s.DateTime.Day >= firstSlot.DateTime.Day)
            //        .OrderBy(s => s.DateTime)
            //        .Take(nSlots);

            //    return stopOnClosedSlot ? query.TakeWhile(s => s.IsOpen) : query;
            //}


        }

        private static int CalculateRequiredSlots(Slot rootSlot, Movie movie)
        {
            var requiredSlots = (int)Math.Ceiling((double)movie.Runtime / 60);

            if (rootSlot.DateTime.Hour + requiredSlots > 23)
            {
                requiredSlots = 23 - rootSlot.DateTime.Hour + 1;
            }

            return requiredSlots;
        }
        



        #region Required repositories
        
        private IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        private IRepository<Schedule> Schedules
        {
            get { return _unitOfWork.Schedules; }
        }

        private IRepository<Movie> Movies
        {
            get { return _unitOfWork.Movies; }
        }

        private IRepository<Slot> Slots
        {
            get { return _unitOfWork.Slots; }
        }

        #endregion
    }
}
