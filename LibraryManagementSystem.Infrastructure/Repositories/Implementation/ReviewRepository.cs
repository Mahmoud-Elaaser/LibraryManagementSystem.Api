using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Context;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Infrastructure.Repositories.Implementation
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewRepository> _logger;

        public ReviewRepository(ApplicationDbContext context, ILogger<ReviewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<Review> GetAllAsync()
        {
            return _context.Reviews
                .Include(r => r.Book)
                .AsNoTracking();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetBookReviewsAsync(int bookId)
        {
            return await _context.Reviews
                .Include(r => r.Book)
                .Where(r => r.BookId == bookId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<Review> AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added new review with ID {ReviewId} for book {BookId}", review.Id, review.BookId);
            return review;
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated review with ID {ReviewId}", review.Id);
        }

        public async Task DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted review with ID {ReviewId}", review.Id);
        }

        public async Task<double> GetAverageRatingForBookAsync(int bookId)
        {
            return await _context.Reviews
                .Where(r => r.BookId == bookId)
                .AverageAsync(r => r.Rating);
        }
    }
}
