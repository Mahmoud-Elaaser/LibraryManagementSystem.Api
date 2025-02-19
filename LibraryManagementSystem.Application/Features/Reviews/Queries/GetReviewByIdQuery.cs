using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Reviews.Queries
{
    public class GetReviewByIdQuery : IRequest<ReviewDto>
    {
        public int Id { get; set; }
    }
}
