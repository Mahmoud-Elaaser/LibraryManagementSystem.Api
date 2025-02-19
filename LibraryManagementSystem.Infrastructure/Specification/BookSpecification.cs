using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Infrastructure.Specification
{
    public class BookSpecification : BaseSpecification<Book>
    {
        public BookSpecification(BookSpecParams specParams)
            : base(x => (string.IsNullOrEmpty(specParams.Search) || x.Title.Contains(specParams.Search))
                      && (!specParams.GenreId.HasValue || x.GenreId == specParams.GenreId)
                      && (!specParams.AuthorId.HasValue || x.AuthorId == specParams.AuthorId))
        {
            AddInclude(x => x.Author);
            AddInclude(x => x.Genre);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }
    }
}
