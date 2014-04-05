using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineStub.Data.Contracts;
using CineStub.Model;

namespace CineStub.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            Context = new CineStubContext();
        }

        public CineStubContext Context { get; private set; }

        private IRepository<Schedule> _schedules;
        private IRepository<Movie> _movies;
        private IRepository<Slot> _slots;
        private IRepository<Actor> _actors; 

        public IRepository<Schedule> Schedules
        {
            get { return _schedules ?? (_schedules = new BaseRepository<Schedule>(Context)); }
        }
        public IRepository<Movie> Movies
        {
            get { return _movies ?? (_movies = new BaseRepository<Movie>(Context)); }
        }
        public IRepository<Slot> Slots
        {
            get { return _slots ?? (_slots = new BaseRepository<Slot>(Context)); }
        }
        public IRepository<Actor> Actors
        {
            get { return _actors ?? (_actors = new BaseRepository<Actor>(Context)); }
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
