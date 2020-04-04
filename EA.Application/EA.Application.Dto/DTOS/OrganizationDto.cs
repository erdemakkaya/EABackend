using EA.Application.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EA.Application.Dto.DTOS
{
    public class OrganizationDto : DtoBase
    {
        /// <summary>
        /// Firmanın Adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Firmanın vergi numarası
        /// </summary>
        public string TaxNumber { get; set; }

        /// <summary>
        /// Firmanın vergi dairesi
        /// </summary>
        public string TaxOffice { get; set; }

        /// <summary>
        /// Firmanın adres bilgisi
        /// </summary>
        public string Address { get; set; }
    }
}
