using LibraryManagementSystem.Application.DTOs.Auth;

namespace LibraryManagementSystem.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequest model);
        Task<AuthResponseDto> LoginAsync(LoginRequest model);
        Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordRequest model);
        Task<AuthResponseDto> ChangePasswordAsync(string userId, ChangePasswordRequest model);
        Task<AuthResponseDto> AssignRoleAsync(string userId, string roleName);
        Task<AuthResponseDto> UpdateRoleAsync(string userId, string oldRoleName, string newRoleName);
        Task<AuthResponseDto> DeleteRoleFromUserAsync(string userId, string roleName);
    }
}
