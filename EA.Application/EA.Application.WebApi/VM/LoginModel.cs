using System.ComponentModel.DataAnnotations;

namespace EA.Application.WebApi.VM
{
    /// <summary>
    /// Login için kullanıcıdan istenen bilgileri taşır
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Email alanı zorunludur 
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Parola alanı zorunludur
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}