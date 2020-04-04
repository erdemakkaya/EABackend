using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EA.Application.Common.Data;
using EA.Application.Common.Enttiy;

namespace EA.Application.Data.Entitites
{
    /// <summary>
    /// Uygulamamızda desteklediğimiz dilleri tutan sınıfımız
    /// </summary>
    public class Language : EntityBase,IEntity
    {
        /// <summary>
        /// Dil adı
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        /// <summary>
        /// Dil kodu. Örneğin TR-tr, EN-us .. vs
        /// </summary>
        [Required(ErrorMessage = "Culture is required")]
        public string Culture { get; set; }
    }
}
