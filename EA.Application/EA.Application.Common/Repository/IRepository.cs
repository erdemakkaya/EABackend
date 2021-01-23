using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EA.Application.Common.Enttiy;

namespace EA.Application.Common.Repository
{
    public interface IRepository<TEntity>: IRepository where TEntity : class,IEntity
    {

        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void InsertOrUpdate(TEntity entity);
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> where);
        TEntity Get(Expression<Func<TEntity, bool>> where);
        TEntity Get(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, IEntity>>[] include);
        TEntity Get(Expression<Func<TEntity, bool>> where, params string[] include);
        IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where = null);
        IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, IEntity>>[] include);
        IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where, params string[] include);
        IEnumerable<TEntity> GetAll();

        TEntity Find(Guid id);
        void Delete(Guid id);

    }
    public interface IRepository { }
}
