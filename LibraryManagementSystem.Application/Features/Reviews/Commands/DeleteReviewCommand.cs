using MediatR;

namespace LibraryManagementSystem.Application.Features.Reviews.Commands
{
    public class DeleteReviewCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
