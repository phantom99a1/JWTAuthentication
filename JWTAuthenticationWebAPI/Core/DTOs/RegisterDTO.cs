using System.ComponentModel.DataAnnotations;

namespace JWTAuthenticationWebAPI.Core.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
