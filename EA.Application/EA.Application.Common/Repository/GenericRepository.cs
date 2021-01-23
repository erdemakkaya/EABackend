using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using EA.Application.Common.Enttiy;

namespace EA.Application.Common.Repository
{
    public class GenericRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IEntity
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(TDbContext context)
        {
            _dbContext = context;
            _dbset = _dbContext.Set<TEntity>();
        }

        protected TDbContext Context => _dbContext;
        public virtual void Insert(TEntity entity)
        {
            _dbset.Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            _dbset.AddRange(entities);
            _dbContext.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            //_dbset.Attach(entity); - audit kısmında orjinal ve değişen değerleri almamızda sorun oluyordu, commentlendi
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public virtual void InsertOrUpdate(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbset.Add(entity);
            }
            else
            {
                //_dbset.Attach(entity); - audit kısmında orjinal ve değişen değerleri almamızda sorun oluyordu, commentlendi
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            var objects = _dbset.Where(where).AsEnumerable();
            _dbset.RemoveRange(objects);
            _dbContext.SaveChanges();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, IEntity>>[] include)
        {
            var q = _dbset.Where(where);
            for (int i = 0; i < include.Length; i++)
            {
                q = q.Include(include[i]);
            }
            return q.FirstOrDefault();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where, params string[] include)
        {
            var q = _dbset.Where(where);
            for (int i = 0; i < include.Length; i++)
            {
                q = q.Include(include[i]);
            }
            return q.FirstOrDefault();
        }

        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where = null)
        {
            return (where == null ? _dbset : _dbset.Where(where));
        }

        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, IEntity>>[] include)
        {
            var q = GetMany(where);
            for (int i = 0; i < include.Length; i++)
            {
                q = q.Include(include[i]);
            }
            return q;
        }

        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where, params string[] include)
        {
            var q = GetMany(where);
            for (int i = 0; i < include.Length; i++)
            {
                q = q.Include(include[i]);
            }
            return q;
        }
        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbset.ToList();
        }

        public TEntity Find(Guid id)
        {
            return _dbset.Find(id);
        }

        public void Delete(Guid id)
        {
            var entity = Find(id); 
            _dbset.Remove(entity);
        }
    }
}
