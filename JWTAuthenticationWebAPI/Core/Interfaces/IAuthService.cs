using JWTAuthenticationWebAPI.Core.DTOs;

namespace JWTAuthenticationWebAPI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDTO> SeedRolesAsync();
        Task<AuthServiceResponseDTO> RegisterAsync(RegisterDTO registerDto);
        Task<AuthServiceResponseDTO> LoginAsync(LoginDTO loginDto);
        Task<AuthServiceResponseDTO> MakeAdminAsync(UpdatePermissionDTO updatePermissionDto);
        Task<AuthServiceResponseDTO> MakeOwnerAsync(UpdatePermissionDTO updatePermissionDto);
    }
}
