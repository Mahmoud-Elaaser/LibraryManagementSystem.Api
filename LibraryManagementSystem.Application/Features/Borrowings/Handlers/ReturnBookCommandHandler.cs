using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Borrowings.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Borrowings.Handlers
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, BorrowingDto>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ReturnBookCommandHandler> _logger;

        public ReturnBookCommandHandler(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            ILogger<ReturnBookCommandHandler> logger)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BorrowingDto> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Borrowing with ID {request.Id} not found");

            if (borrowing.IsReturned)
                throw new InvalidOperationException($"Book is already returned");

            var book = await _bookRepository.GetByIdAsync(borrowing.BookId);
            var user = await _userRepository.GetByIdAsync(borrowing.UserId);

            borrowing.ReturnDate = request.ReturnDate;
            borrowing.IsReturned = true;

            book.IsAvailable = true;
            await _bookRepository.UpdateAsync(book);
            await _borrowingRepository.UpdateAsync(borrowing);

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
                Status = "Returned"
            };
        }
    }
}
