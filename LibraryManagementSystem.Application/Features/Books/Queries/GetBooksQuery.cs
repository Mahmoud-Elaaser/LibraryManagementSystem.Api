using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Books.Queries
{
    public class GetBooksQuery : IRequest<IEnumerable<BookDto>>
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
