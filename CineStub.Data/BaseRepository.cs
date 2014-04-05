using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CineStub.Data.Contracts;
using CineStub.Model;

namespace CineStub.Data
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        public BaseRepository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        protected DbContext DbContext { get; set; }
        protected DbSet<T> DbSet { get; set; } 

        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeExpressions)
        {
            return includeExpressions.Aggregate<Expression<Func<T, object>>, IQueryable<T>>(DbSet, (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<T> GetAllIncluding(string[] includePaths)
        {
            return includePaths.Aggregate<string, IQueryable<T>>(DbSet, (current, includePath) => current.Include(includePath));
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual T GetByIdIncluding(int id, params Expression<Func<T, object>>[] includeExpressions)
        {
            var query = includeExpressions.Aggregate<Expression<Func<T, object>>, IQueryable<T>>(DbSet, (current, includeExpression) => current.Include(includeExpression));

            return query.FirstOrDefault(t => t.Id == id);
        }

        public virtual T GetByIdIncluding(int id, string[] includePaths)
        {
            var query = includePaths.Aggregate<string, IQueryable<T>>(DbSet, (current, includePath) => current.Include(includePath));

            return query.FirstOrDefault(t => t.Id == id);
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(int id)
        {
            var entity = GetById(id);
            Remove(entity);
        }

        public virtual void Remove(T entity)
        {
            DbSet.Remove(entity);
        }
    }
}
