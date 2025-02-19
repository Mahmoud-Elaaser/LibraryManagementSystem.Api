using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Infrastructure.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
        Task<Genre> AddAsync(Genre genre);
        Task<Genre> UpdateAsync(Genre genre);
        Task DeleteAsync(Genre genre);
    }
}
