using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Context;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace LibraryManagementSystem.Infrastructure.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<UserRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Borrowings)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Borrowings)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Borrowings)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Borrowings)
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersWithOverdueBooksAsync()
        {
            return await _context.Users
                .Include(u => u.Borrowings)
                .Where(u => u.Borrowings.Any(b => !b.IsReturned && b.DueDate < DateTime.UtcNow))
                .ToListAsync();
        }

        public async Task<User> AddAsync(User user, string password)
        {
            try
            {
                user.MembershipDate = DateTime.UtcNow;
                user.IsActive = true;
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new ApplicationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                // Assign default role
                await _userManager.AddToRoleAsync(user, "User");

                _logger.LogInformation("Created new user with ID {UserId}", user.Id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with email {Email}", user.Email);
                throw;
            }
        }

        public async Task<User> UpdateAsync(User user)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    throw new ApplicationException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                _logger.LogInformation("Updated user with ID {UserId}", user.Id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", user.Id);
                throw;
            }
        }

        public async Task DeleteAsync(User user)
        {
            try
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    throw new ApplicationException($"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                _logger.LogInformation("Deleted user with ID {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", user.Id);
                throw;
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<int> GetActiveBorrowingsCountAsync(int userId)
        {
            return await _context.Borrowings
                .CountAsync(b => b.UserId == userId && !b.IsReturned);
        }
    }
}
