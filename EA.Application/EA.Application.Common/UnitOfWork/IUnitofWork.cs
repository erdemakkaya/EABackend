using System;
using System.Collections.Generic;
using System.Text;
using EA.Application.Common.Enttiy;
using EA.Application.Common.Repository;

namespace EA.Application.Common.UnitOfWork
{
 
    public interface IUnitofWork : IDisposable
    {
     
        IRepository<TEntity> GetDefaultRepo<TEntity>() where TEntity : class, IEntity;
        void Commit();
        void Rollback();

       
        int SaveChanges(bool ensureAutoHistory=false);
    }
}