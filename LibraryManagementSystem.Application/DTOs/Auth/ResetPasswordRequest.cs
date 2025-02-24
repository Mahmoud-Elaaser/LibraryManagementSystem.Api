using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Application.DTOs.Auth
{
    public class ResetPasswordRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string OTP { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
