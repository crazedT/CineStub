using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineStub.Model;

namespace CineStub.Data.Contracts
{
    public interface IUnitOfWork
    {
        CineStubContext Context { get; }

        
        
        IRepository<Schedule> Schedules { get; }
        IRepository<Movie> Movies { get; }
        IRepository<Slot> Slots { get; }
        IRepository<Actor> Actors { get; } 

        void SaveChanges();
    }
}
