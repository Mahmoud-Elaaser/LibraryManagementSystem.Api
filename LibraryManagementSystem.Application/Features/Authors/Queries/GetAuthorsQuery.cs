using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Authors.Queries
{
    public class GetAuthorsQuery : IRequest<IEnumerable<AuthorDto>>
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
