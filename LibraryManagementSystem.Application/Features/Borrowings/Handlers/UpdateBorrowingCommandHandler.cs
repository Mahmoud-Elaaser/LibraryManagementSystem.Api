using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Borrowings.Commands;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Borrowings.Handlers
{
    public class UpdateBorrowingCommandHandler : IRequestHandler<UpdateBorrowingCommand, BorrowingDto>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateBorrowingCommandHandler> _logger;

        public UpdateBorrowingCommandHandler(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            ILogger<UpdateBorrowingCommandHandler> logger)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BorrowingDto> Handle(UpdateBorrowingCommand request, CancellationToken cancellationToken)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Borrowing with ID {request.Id} not found");

            var book = await _bookRepository.GetByIdAsync(borrowing.BookId);
            var user = await _userRepository.GetByIdAsync(borrowing.UserId);

            borrowing.DueDate = request.DueDate;
            if (request.ReturnDate.HasValue)
            {
                borrowing.ReturnDate = request.ReturnDate;
                borrowing.IsReturned = true;
                book.IsAvailable = true;
                await _bookRepository.UpdateAsync(book);
            }

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
