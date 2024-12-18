using System.ComponentModel.DataAnnotations;

namespace JWTAuthenticationWebAPI.Core.DTOs
{
    public class UpdatePermissionDTO
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;
    }
}
