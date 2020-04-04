﻿using System;
using System.Collections.Generic;
using System.Text;
using EA.Application.Common.Enttiy;
using EA.Application.Common.Repository;

namespace EA.Application.Common.UnitOfWork
{
    /// <summary>
    /// Oluşturacağımız UnitofWork sınıfının içermesi gereken metotlara ait imzaları bu kısımda tanımlayacağız.
    /// UnitofWork sınıfını türetirken doğrudan değil bu interface'i kullanarak türeteceğiz Böylece dependency injection yaparak 
    /// kodumuza esneklik kazandırmış olacağız.
    /// </summary>
    public interface IUnitofWork : IDisposable
    {
        /// <summary>
        /// Gerektiğinde repository instance oluşturmak için kullanılacak.
        /// </summary>
        /// <typeparam name="T">Hangi entity miz için repository oluşmasını istiyorsak o sınıfı gönderiyoruz</typeparam>
        /// <returns></returns>
        IGenericRepository<T> GetRepository<T>() where T :class, IEntity;

        bool BeginNewTransaction();
        bool RollBackTransaction();

        /// <summary>
        /// Veritabanında işlemlerin yapılması emrini veren kısım olacak
        /// Repository içerisinde kuyruğa aldığımız tüm işlemler bu metot çalıştırıldığı anda sırası ile veritabanında değişikliğe uğrayacak
        /// </summary>
        /// <returns></returns>
        int SaveChanges(bool ensureAutoHistory=false);
    }
}