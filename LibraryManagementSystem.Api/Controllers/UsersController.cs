using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll([FromQuery] bool? activeOnly)
        {
            try
            {
                IEnumerable<User> users;
                if (activeOnly.HasValue && activeOnly.Value)
                {
                    users = await _userRepository.GetActiveUsersAsync();
                }
                else
                {
                    users = await _userRepository.GetAllAsync();
                }

                var userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    var activeBorrowings = await _userRepository.GetActiveBorrowingsCountAsync(user.Id);
                    userDtos.Add(new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        MembershipDate = user.MembershipDate,
                        IsActive = user.IsActive,
                        ActiveBorrowingsCount = activeBorrowings
                    });
                }

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                var activeBorrowings = await _userRepository.GetActiveBorrowingsCountAsync(user.Id);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    MembershipDate = user.MembershipDate,
                    IsActive = user.IsActive,
                    ActiveBorrowingsCount = activeBorrowings
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersWithOverdueBooks()
        {
            try
            {
                var users = await _userRepository.GetUsersWithOverdueBooksAsync();
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var activeBorrowings = await _userRepository.GetActiveBorrowingsCountAsync(user.Id);
                    userDtos.Add(new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        MembershipDate = user.MembershipDate,
                        IsActive = user.IsActive,
                        ActiveBorrowingsCount = activeBorrowings
                    });
                }

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users with overdue books");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                if (await _userRepository.EmailExistsAsync(createUserDto.Email))
                {
                    return BadRequest("Email already exists");
                }

                var user = new User
                {
                    Name = createUserDto.Name,
                    Email = createUserDto.Email,
                    PhoneNumber = createUserDto.PhoneNumber,
                    Address = createUserDto.Address
                };

                var createdUser = await _userRepository.AddAsync(user, user.PasswordHash);
                var userDto = new UserDto
                {
                    Id = createdUser.Id,
                    Name = createdUser.Name,
                    Email = createdUser.Email,
                    PhoneNumber = createdUser.PhoneNumber,
                    Address = createdUser.Address,
                    MembershipDate = createdUser.MembershipDate,
                    IsActive = createdUser.IsActive,
                    ActiveBorrowingsCount = 0
                };

                return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                if (user.Email != updateUserDto.Email && await _userRepository.EmailExistsAsync(updateUserDto.Email))
                {
                    return BadRequest("Email already exists");
                }

                user.Name = updateUserDto.Name;
                user.Email = updateUserDto.Email;
                user.PhoneNumber = updateUserDto.PhoneNumber;
                user.Address = updateUserDto.Address;
                user.IsActive = updateUserDto.IsActive;

                await _userRepository.UpdateAsync(user);
                var activeBorrowings = await _userRepository.GetActiveBorrowingsCountAsync(user.Id);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    MembershipDate = user.MembershipDate,
                    IsActive = user.IsActive,
                    ActiveBorrowingsCount = activeBorrowings
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                var activeBorrowings = await _userRepository.GetActiveBorrowingsCountAsync(user.Id);
                if (activeBorrowings > 0)
                {
                    return BadRequest("Cannot delete user with active borrowings");
                }

                await _userRepository.DeleteAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
