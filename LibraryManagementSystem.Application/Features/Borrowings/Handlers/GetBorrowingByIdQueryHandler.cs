using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Borrowings.Queries;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Borrowings.Handlers
{
    public class GetBorrowingByIdQueryHandler : IRequestHandler<GetBorrowingByIdQuery, BorrowingDto>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetBorrowingByIdQueryHandler> _logger;

        public GetBorrowingByIdQueryHandler(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            ILogger<GetBorrowingByIdQueryHandler> logger)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BorrowingDto> Handle(GetBorrowingByIdQuery request, CancellationToken cancellationToken)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Borrowing with ID {request.Id} not found");

            var book = await _bookRepository.GetByIdAsync(borrowing.BookId);
            var user = await _userRepository.GetByIdAsync(borrowing.UserId);

            return new BorrowingDto
            {
                Id = borrowing.Id,
                BookId = borrowing.BookId,
                BookTitle = book?.Title,
                UserId = borrowing.UserId,
                UserName = user?.Name,
                BorrowDate = borrowing.BorrowDate,
                DueDate = borrowing.DueDate,
                ReturnDate = borrowing.ReturnDate,
                IsReturned = borrowing.IsReturned,
                Status = GetBorrowingStatus(borrowing)
            };
        }

        private string GetBorrowingStatus(Borrowing borrowing)
        {
            if (borrowing.IsReturned) return "Returned";
            if (borrowing.DueDate < DateTime.UtcNow) return "Overdue";
            return "Active";
        }
    }
}
