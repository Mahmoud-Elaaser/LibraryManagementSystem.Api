using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Context;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories.Implementation
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Borrowing>> GetAllAsync()
        {
            return await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<Borrowing> GetByIdAsync(int id)
        {
            return await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Borrowing>> GetByUserIdAsync(int userId)
        {
            return await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Borrowing>> GetActiveByUserIdAsync(int userId)
        {
            return await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.User)
                .Where(b => b.UserId == userId && !b.IsReturned)
                .ToListAsync();
        }

        public async Task<IEnumerable<Borrowing>> GetOverdueBorrowingsAsync()
        {
            return await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.User)
                .Where(b => !b.IsReturned && b.DueDate < DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<Borrowing> AddAsync(Borrowing borrowing)
        {
            await _context.Borrowings.AddAsync(borrowing);
            await _context.SaveChangesAsync();
            return borrowing;
        }

        public async Task<Borrowing> UpdateAsync(Borrowing borrowing)
        {
            _context.Entry(borrowing).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return borrowing;
        }

        public async Task DeleteAsync(Borrowing borrowing)
        {
            _context.Borrowings.Remove(borrowing);
            await _context.SaveChangesAsync();
        }
    }
}
