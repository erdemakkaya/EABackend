using System;
using EA.Application.Common.Enttiy;
using EA.Application.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace EA.Application.Data.Entitites
{

    /// <summary>
    /// Bu sınıf IdentityRole sınıfından kalıtım alır
    /// Sistemde bulunan tüm roller bu sınıf ile kontrol edilecek
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>, IEntity,IFullAudited
    {
        private AppStatus status;

        /// <summary>
        /// Mevcut IdentityRole propertylerine ek propertyler eklemek istiyorsak bu alanda istediğimiz gibi tanımlama yapabiliriz.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Bu rolü kimin oluşturduğu bilgisi
        /// </summary>
        public Guid? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
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