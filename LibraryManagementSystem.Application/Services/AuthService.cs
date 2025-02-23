using AutoMapper;
using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace LibraryManagementSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserRepository userRepository,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<AuthService> logger,
            RoleManager<IdentityRole<int>> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequest model)
        {
            try
            {
                if (await _userRepository.EmailExistsAsync(model.Email))
                {
                    return new AuthResponseDto { IsSuccess = false, Message = "Email already exists" };
                }
                var user = _mapper.Map<User>(model);
                user.UserName = model.Email;
                user.IsActive = false;
                user.MembershipDate = DateTime.UtcNow;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return new AuthResponseDto { IsSuccess = false, Message = result.Errors.First().Description };
                }
                await _userManager.AddToRoleAsync(user, "User");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);


                var baseUrl = _configuration["AppUrl"]?.TrimEnd('/');
                var confirmationLink = $"{baseUrl}/api/auth/confirm-email?token={WebUtility.UrlEncode(token)}&email={WebUtility.UrlEncode(user.Email)}";

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Confirm your email",
                    $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>"
                );
                return new AuthResponseDto { IsSuccess = true, Message = "Registration successful. Please check your email for confirmation." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email {Email}", model.Email);
                throw;
            }
        }
        public async Task<AuthResponseDto> LoginAsync(LoginRequest model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return new AuthResponseDto { IsSuccess = false, Message = "Invalid credentials" };

                if (!user.IsActive)
                    return new AuthResponseDto { IsSuccess = false, Message = "Account is not active" };

                if (!await _userManager.IsEmailConfirmedAsync(user))
                    return new AuthResponseDto { IsSuccess = false, Message = "Please confirm your email first" };

                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                    return new AuthResponseDto { IsSuccess = false, Message = "Invalid credentials" };

                var token = await GenerateJwtTokenAsync(user);
                var refreshToken = GenerateRefreshToken();

                //user.RefreshToken = refreshToken;
                //user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userManager.UpdateAsync(user);

                return new AuthResponseDto
                {
                    IsSuccess = true,
                    Token = token.Token,
                    RefreshToken = refreshToken,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email {Email}", model.Email);
                throw;
            }
        }

        public async Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return new AuthResponseDto { IsSuccess = false, Message = "User not found" };

                /// Generate password reset token and OTP
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var otp = GenerateOTP();

                /// Store OTP in user's security stamp
                user.SecurityStamp = otp;
                await _userManager.UpdateAsync(user);


                var emailBody = $"Your OTP for password reset is: {otp}";
                await _emailService.SendEmailAsync(user.Email, "Password Reset OTP", emailBody);

                return new AuthResponseDto { IsSuccess = true, Message = "OTP sent to your email" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during forgot password for email {Email}", model.Email);
                throw;
            }
        }

        public async Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordRequest model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return new AuthResponseDto { IsSuccess = false, Message = "User not found" };

                /// Verify OTP
                if (user.SecurityStamp != model.Token)
                    return new AuthResponseDto { IsSuccess = false, Message = "Invalid OTP" };

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (!result.Succeeded)
                    return new AuthResponseDto { IsSuccess = false, Message = result.Errors.First().Description };

                /// Clear the security stamp
                user.SecurityStamp = Guid.NewGuid().ToString();
                await _userManager.UpdateAsync(user);

                return new AuthResponseDto { IsSuccess = true, Message = "Password reset successful" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset for email {Email}", model.Email);
                throw;
            }
        }

        public async Task<AuthResponseDto> ChangePasswordAsync(string userId, ChangePasswordRequest model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return new AuthResponseDto { IsSuccess = false, Message = "User not found" };

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                    return new AuthResponseDto { IsSuccess = false, Message = result.Errors.First().Description };

                return new AuthResponseDto { IsSuccess = true, Message = "Password changed successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password change for user {UserId}", userId);
                throw;
            }
        }

        public async Task<AuthResponseDto> AssignRoleAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return new AuthResponseDto { IsSuccess = false, Message = "User not found" };

                if (!await _roleManager.RoleExistsAsync(roleName))
                    return new AuthResponseDto { IsSuccess = false, Message = "Role does not exist" };

                if (await _userManager.IsInRoleAsync(user, roleName))
                    return new AuthResponseDto { IsSuccess = false, Message = "User already has this role" };

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                    return new AuthResponseDto { IsSuccess = false, Message = result.Errors.First().Description };

                return new AuthResponseDto { IsSuccess = true, Message = "Role assigned successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role {Role} to user {UserId}", roleName, userId);
                throw;
            }
        }

        private async Task<TokenDto> GenerateJwtTokenAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = token.ValidTo
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateOTP()
        {
            return new Random().Next(100000, 999999).ToString();
        }


    }
}