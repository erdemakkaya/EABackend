using EA.Application.Data.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace EA.Application.Dto.DTOS
{
    public class ApplicationUserRoleDto
    {
        /// <summary>
        /// Kullanıcı bilgisi
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Role bilgisi
        /// </summary>
        public ApplicationRole Role { get; set; }
    }
}
