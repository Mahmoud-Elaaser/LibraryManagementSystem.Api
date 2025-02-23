using LibraryManagementSystem.Application.DTOs.Auth;
using LibraryManagementSystem.Application.Services;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHtmlResponseService _htmlResponseService;
        private readonly UserManager<User> _userManager;
        public AuthController(IAuthService authService, UserManager<User> userManager, IHtmlResponseService htmlResponseService)
        {
            _authService = authService;
            _userManager = userManager;
            _htmlResponseService = htmlResponseService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var result = await _authService.RegisterAsync(model);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var result = await _authService.LoginAsync(model);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var result = await _authService.ForgotPasswordAsync(model);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var result = await _authService.ResetPasswordAsync(model);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _authService.ChangePasswordAsync(userId, model);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("roles/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest model)
        {
            var result = await _authService.AssignRoleAsync(model.UserId, model.RoleName);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("roles/update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest model)
        {
            var result = await _authService.UpdateRoleAsync(model.UserId, model.OldRoleName, model.NewRoleName);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("roles/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleRequest model)
        {
            var result = await _authService.DeleteRoleFromUserAsync(model.UserId, model.RoleName);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }


        /*
            This endpoint validates the token and marks your email as confirmed in the database  
            and If you don't have this endpoint, clicking the link in the email won't work
        */
        [HttpGet("confirm-email")]
        public async Task<ContentResult> ConfirmEmail(string token, string email)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                {
                    return _htmlResponseService.CreateHtmlResponse("Invalid email confirmation token", false);
                }

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return _htmlResponseService.CreateHtmlResponse("User not found", false);
                }

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    user.IsActive = true;
                    await _userManager.UpdateAsync(user);

                    return _htmlResponseService.CreateHtmlResponse("Email confirmed successfully! You can now close this page and log in to your account.", true);
                }

                return _htmlResponseService.CreateHtmlResponse("Email confirmation failed", false);
            }
            catch (Exception ex)
            {
                return _htmlResponseService.CreateHtmlResponse("An error occurred while confirming your email", false);
            }
        }



    }
}
