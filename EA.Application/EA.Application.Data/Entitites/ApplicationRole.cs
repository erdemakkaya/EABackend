﻿using System;
using EA.Application.Common.Enttiy;
using EA.Application.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace EA.Application.Data.Entitites
{

    /// <summary>
    /// Bu sınıf IdentityRole sınıfından kalıtım alır
    /// Sistemde bulunan tüm roller bu sınıf ile kontrol edilecek
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>, IEntity
    {
        private AppStatus status;
        private DateTime createdDate;

        /// <summary>
        /// Mevcut IdentityRole propertylerine ek propertyler eklemek istiyorsak bu alanda istediğimiz gibi tanımlama yapabiliriz.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Bu rolü kimin oluşturduğu bilgisi
        /// </summary>
        public DateTime? CreateDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                createdDate = value ?? DateTime.UtcNow;
            }
        }

        public Guid? Creator { get; set; }

        public AppStatus? Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value ?? AppStatus.Aktif;
            }
        }
    }
}