using System;
using System.Collections.Generic;
using System.Text;
using EA.Application.Common.Enums;

namespace EA.Application.Common.Dto
{
    public class DtoBase
    {
        /// <summary>
        /// Bu sınıfı inherit alacak derived class'larımızın içereceği alanları yazıyoruz
        /// Böylece tekrarlı kod yazmanın önüne geçmiş olacağız. 
        /// </summary>
        public Guid Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? Creator { get; set; }
        public AppStatus? Status { get; set; }
    }
}
