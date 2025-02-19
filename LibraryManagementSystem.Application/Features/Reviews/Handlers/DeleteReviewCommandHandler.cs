using LibraryManagementSystem.Application.Features.Reviews.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Reviews.Handlers
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<DeleteReviewCommandHandler> _logger;

        public DeleteReviewCommandHandler(
            IReviewRepository reviewRepository,
            ILogger<DeleteReviewCommandHandler> logger)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Review with ID {request.Id} not found");

            await _reviewRepository.DeleteAsync(review);
            return true;
        }
    }
}
