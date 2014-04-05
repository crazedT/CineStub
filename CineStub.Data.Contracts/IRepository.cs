using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CineStub.Model;

namespace CineStub.Data.Contracts
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeExpressions);

        IQueryable<T> GetAllIncluding(string[] includePaths);

        T GetById(int id);

        T GetByIdIncluding(int id, params Expression<Func<T, object>>[] includeExpressions);

        T GetByIdIncluding(int id, string[] includePaths);
        
        void Add(T entity);

        void Update(T entity);

        void Remove(int id);

        void Remove(T entity);
    }
}
