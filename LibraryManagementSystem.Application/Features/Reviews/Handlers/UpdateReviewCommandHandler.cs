using AutoMapper;
using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Reviews.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Reviews.Handlers
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewDto>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateReviewCommandHandler> _logger;

        public UpdateReviewCommandHandler(
            IReviewRepository reviewRepository,
            IBookRepository bookRepository,
            IMapper mapper,
            ILogger<UpdateReviewCommandHandler> logger)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReviewDto> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Review with ID {request.Id} not found");

            var book = await _bookRepository.GetByIdAsync(review.BookId)
                ?? throw new KeyNotFoundException($"Book with ID {review.BookId} not found");

            _mapper.Map(request, review);
            await _reviewRepository.UpdateAsync(review);

            var reviewDto = _mapper.Map<ReviewDto>(review);
            reviewDto.BookTitle = book.Title;

            return reviewDto;
        }
    }
}
