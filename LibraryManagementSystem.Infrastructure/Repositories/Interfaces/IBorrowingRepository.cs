using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Infrastructure.Repositories.Interfaces
{
    public interface IBorrowingRepository
    {
        Task<IEnumerable<Borrowing>> GetAllAsync();
        Task<Borrowing> GetByIdAsync(int id);
        Task<IEnumerable<Borrowing>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Borrowing>> GetActiveByUserIdAsync(int userId);
        Task<IEnumerable<Borrowing>> GetOverdueBorrowingsAsync();
        Task<Borrowing> AddAsync(Borrowing borrowing);
        Task<Borrowing> UpdateAsync(Borrowing borrowing);
        Task DeleteAsync(Borrowing borrowing);
    }
}
