using System;
using System.Collections.Generic;
using System.ComponentModel;
using EA.Application.Common.Enttiy;
using EA.Application.Common.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EA.Application.Common.UnitOfWork
{

    public class UnitofWork<TDbContext> : IUnitofWork
    where TDbContext : DbContext
    {

        private readonly TDbContext _context;

        private IDbContextTransaction _transaction;
        private bool _disposed;

        /// <summary>
        /// Yapıcı method dependency injection ile DbContext nesnesi türetiyor
        /// </summary>
        /// <param name="context"></param>
        public UnitofWork(TDbContext context)
        {
            _context = context;
            _transaction = _context.Database.BeginTransaction();
        }

        public IRepository<TEntity> GetDefaultRepo<TEntity>() where TEntity : class, IEntity
        {
            return new GenericRepository<TDbContext, TEntity>(_context);
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }
        public int SaveChanges(bool ensureAutoHistory = false)
        {
            var transaction = _transaction != null ? _transaction : _context.Database.BeginTransaction();
            using (transaction)
            {
                try
                {
                    if (_context == null)
                    {
                        throw new ArgumentException("Context is null");
                    }

                    if (ensureAutoHistory)
                    {
                        //_context.EnsureAutoHistory();
                    }
                    int result = _context.SaveChanges();
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error on save changes ", ex);
                }
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
