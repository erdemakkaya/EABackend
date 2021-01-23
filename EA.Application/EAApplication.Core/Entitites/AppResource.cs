using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EA.Application.Common.Data;
using EA.Application.Common.Enttiy;
using EA.Application.Data.Entitites;

namespace EA.Application.Data.Entitites
{
    /// <summary>
    /// Çoklu dil alt yapısına sahip projemizin sözlüğü görevini görecek olan sınıfımız.
    /// Kelimelerin hangi diller ne anlama geldiği bu sınıf üzerinde tutulacak
    /// </summary>
    public class AppResource : EntityBase, IEntity
    {
    
        [Required(ErrorMessage = "Key is required")]
        public string Key { get; set; }

        
        public Guid LanguageId { get; set; }

     
        public virtual Language Language { get; set; }

        /// <summary>
        /// Value ise verilen key in belirlenen dil için çevirisini tutacak
        /// </summary>
        [Required(ErrorMessage = "Value is required")]
        public string Value { get; set; }
    }
}
