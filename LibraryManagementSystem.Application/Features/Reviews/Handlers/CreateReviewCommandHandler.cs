using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ILogger<CreateReviewCommandHandler> _logger;

        public CreateReviewCommandHandler(
            IReviewRepository reviewRepository,
            IBookRepository bookRepository,
            IMapper mapper,
            ILogger<CreateReviewCommandHandler> logger)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId)
                ?? throw new KeyNotFoundException($"Book with ID {request.BookId} not found");

            var review = _mapper.Map<Review>(request);
            review.ReviewDate = DateTime.UtcNow;

            var createdReview = await _reviewRepository.AddAsync(review);
            var reviewDto = _mapper.Map<ReviewDto>(createdReview);
            reviewDto.BookTitle = book.Title;

            return reviewDto;
        }
    }
}
