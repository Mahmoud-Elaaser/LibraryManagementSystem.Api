using AutoMapper;
using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Borrowings.Commands;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Borrowings.Handlers
{
    public class CreateBorrowingCommandHandler : IRequestHandler<CreateBorrowingCommand, BorrowingDto>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBorrowingCommandHandler> _logger;

        public CreateBorrowingCommandHandler(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<CreateBorrowingCommandHandler> logger)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BorrowingDto> Handle(CreateBorrowingCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId)
                ?? throw new KeyNotFoundException($"Book with ID {request.BookId} not found");

            var user = await _userRepository.GetByIdAsync(request.UserId)
                ?? throw new KeyNotFoundException($"User with ID {request.UserId} not found");

            if (!book.IsAvailable)
                throw new InvalidOperationException($"Book with ID {request.BookId} is not available for borrowing");

            var borrowing = new Borrowing
            {
                BookId = request.BookId,
                UserId = request.UserId,
                BorrowDate = request.BorrowDate,
                DueDate = request.DueDate,
                IsReturned = false
            };

            book.IsAvailable = true;
            await _bookRepository.UpdateAsync(book);

            var createdBorrowing = await _borrowingRepository.AddAsync(borrowing);

            return new BorrowingDto
            {
                Id = createdBorrowing.Id,
                BookId = createdBorrowing.BookId,
                BookTitle = book.Title,
                UserId = createdBorrowing.UserId,
                UserName = user.Name,
                BorrowDate = createdBorrowing.BorrowDate,
                DueDate = createdBorrowing.DueDate,
                ReturnDate = createdBorrowing.ReturnDate,
                IsReturned = createdBorrowing.IsReturned,
                Status = GetBorrowingStatus(createdBorrowing)
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
