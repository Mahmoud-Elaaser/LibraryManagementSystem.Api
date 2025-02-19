using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Context;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Infrastructure.Repositories.Implementation
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthorRepository> _logger;

        public AuthorRepository(
            ApplicationDbContext context,
            ILogger<AuthorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<Author> GetAllAsync()
        {
            return _context.Authors
                .Include(a => a.Books)
                .AsNoTracking();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found", id);
            }

            return author;
        }

        public async Task<Author> AddAsync(Author author)
        {
            try
            {
                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Added new author with ID {AuthorId}", author.Id);

                return author;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding author: {Message}", ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(Author author)
        {
            try
            {
                _context.Entry(author).State = EntityState.Modified;

                // Prevent the Books collection from being modified
                _context.Entry(author).Collection(a => a.Books).IsModified = false;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Updated author with ID {AuthorId}", author.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict updating author {AuthorId}", author.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating author {AuthorId}: {Message}", author.Id, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(Author author)
        {
            try
            {
                if (await _context.Books.AnyAsync(b => b.Id == author.Id))
                {
                    throw new InvalidOperationException($"Cannot delete author {author.Id} because they have associated books");
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted author with ID {AuthorId}", author.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting author {AuthorId}: {Message}", author.Id, ex.Message);
                throw;
            }
        }

        // Additional helper methods

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Authors.AnyAsync(a => a.Id == id);
        }

        public async Task<int> GetAuthorBookCountAsync(int authorId)
        {
            return await _context.Books.CountAsync(b => b.Id == authorId);
        }

        public async Task<IEnumerable<Author>> GetAuthorsByIdsAsync(IEnumerable<int> authorIds)
        {
            return await _context.Authors
                .Where(a => authorIds.Contains(a.Id))
                .Include(a => a.Books)
                .ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsWithBookCountGreaterThanAsync(int bookCount)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .Where(a => a.Books.Count > bookCount)
                .ToListAsync();
        }

        public async Task<IDictionary<string, int>> GetAuthorCountByNationalityAsync()
        {
            return await _context.Authors
                .GroupBy(a => a.Nationality)
                .Select(g => new { Nationality = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Nationality, x => x.Count);
        }
    }
}
