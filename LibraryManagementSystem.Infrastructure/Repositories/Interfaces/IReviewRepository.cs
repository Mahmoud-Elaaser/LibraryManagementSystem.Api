using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Infrastructure.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        IQueryable<Review> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetBookReviewsAsync(int bookId);
        Task<Review> AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Review review);
        Task<double> GetAverageRatingForBookAsync(int bookId);
    }
}
