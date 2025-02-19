using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Reviews.Commands;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Reviews.Handlers
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<CreateReviewCommandHandler> _logger;

        public CreateReviewCommandHandler(
            IReviewRepository reviewRepository,
            IBookRepository bookRepository,
            ILogger<CreateReviewCommandHandler> logger)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId)
                ?? throw new KeyNotFoundException($"Book with ID {request.BookId} not found");

            var review = new Review
            {
                BookId = request.BookId,
                ReviewerName = request.ReviewerName,
                Rating = request.Rating,
                Comment = request.Comment,
                ReviewDate = DateTime.UtcNow
            };

            var createdReview = await _reviewRepository.AddAsync(review);

            return new ReviewDto
            {
                Id = createdReview.Id,
                BookId = createdReview.BookId,
                BookTitle = book.Title,
                ReviewerName = createdReview.ReviewerName,
                Rating = createdReview.Rating,
                Comment = createdReview.Comment,
                ReviewDate = createdReview.ReviewDate
            };
        }
    }
}
