using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Reviews.Queries
{
    public class GetReviewsQuery : IRequest<IEnumerable<ReviewDto>>
    {
        public int? BookId { get; set; }
        public string? SortBy { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
