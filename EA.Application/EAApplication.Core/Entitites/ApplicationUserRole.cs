using System;
using System.ComponentModel.DataAnnotations;
using EA.Application.Common.Enttiy;
using EA.Application.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace EA.Application.Data.Entitites
{
    /// <summary>
    /// Kullanıcılar ve rollerin ilişkilendirildiği tablo. Many to many bir ilişki kurmak için 3. bir tabloya ihtiyaç duyuluyor
    /// </summary>
    public class ApplicationUserRole : IdentityUserRole<Guid>, IEntity
    {
        public ApplicationUserRole()
        {
            if (Id==Guid.Empty)
            {
                Id=Guid.NewGuid();
            }
        }
        private AppStatus status;
        private DateTime createdDate;

        [Key]
        public Guid Id { get; set; }
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Role bilgisi
        /// </summary>
        public virtual ApplicationRole Role { get; set; }

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
