using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Reviews.Commands
{
    public class UpdateReviewCommand : IRequest<ReviewDto>
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
