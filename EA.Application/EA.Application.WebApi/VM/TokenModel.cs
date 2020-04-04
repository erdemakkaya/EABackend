using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EA.Application.Dto.DTOS;

namespace EA.Application.WebApi.VM
{
    /// <summary>
    /// Kullanıcıya geri döneceğimiz token bilgilerini taşıyacak
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// Object türünden token 
        /// </summary>
        public object Token { get; set; }

        /// <summary>
        /// Kullanıcının diğer bilgileri 
        /// </summary>
        public ApplicationUserDto UserDto { get; set; }
    }
}