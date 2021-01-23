using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EA.Application.Common.Data;
using EA.Application.Common.Enttiy;

namespace EA.Application.Data.Entitites
{
    /// <summary>
    /// Müşterilerin bilgilerin, tutan sınıf
    /// </summary>
    public class Customer : EntityBase, IEntity
    {
       
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Name Should be minimum 3 characters and a maximum 100 characters")]
        public string Name { get; set; }

     
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Surname Should be minimum 3 characters and a maximum 100 characters")]
        public string Surname { get; set; }

        
        [Required(ErrorMessage = "Organization is required")]
        [DisplayName(nameof(Organization))]
        public Guid OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }

        [Required(ErrorMessage = "Mail Address is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Mail Address Should be minimum 3 characters and a maximum 100 characters")]
        [DataType(DataType.EmailAddress)]
        public string MailAdress { get; set; }

     
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        /// <summary>
        /// Müşteriye projenin ilerleyen dönemlerinde bir dashboard vs vermek istersek diye birde şifre alanı bırakıyoruz
        /// </summary>
        public string PasswordHash { get; set; }
    }
}
