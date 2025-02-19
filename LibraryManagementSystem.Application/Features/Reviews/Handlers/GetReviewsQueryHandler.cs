using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Reviews.Queries;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Reviews.Handlers
{
    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, IEnumerable<ReviewDto>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<GetReviewsQueryHandler> _logger;

        public GetReviewsQueryHandler(
            IReviewRepository reviewRepository,
            IBookRepository bookRepository,
            ILogger<GetReviewsQueryHandler> logger)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = _reviewRepository.GetAllAsync();
            var reviewDtos = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                var book = await _bookRepository.GetByIdAsync(review.BookId);
                reviewDtos.Add(new ReviewDto
                {
                    Id = review.Id,
                    BookId = review.BookId,
                    BookTitle = book?.Title ?? "Unknown Book",
                    ReviewerName = review.ReviewerName,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ReviewDate = review.ReviewDate
                });
            }

            return reviewDtos;
        }
    }
}
