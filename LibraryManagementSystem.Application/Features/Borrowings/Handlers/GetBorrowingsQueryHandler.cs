using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Borrowings.Queries;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Features.Borrowings.Handlers
{
    public class GetBorrowingsQueryHandler : IRequestHandler<GetBorrowingsQuery, IEnumerable<BorrowingDto>>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetBorrowingsQueryHandler> _logger;

        public GetBorrowingsQueryHandler(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            ILogger<GetBorrowingsQueryHandler> logger)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BorrowingDto>> Handle(GetBorrowingsQuery request, CancellationToken cancellationToken)
        {
            var borrowings = await _borrowingRepository.GetAllAsync();

            if (request.ActiveOnly.HasValue && request.ActiveOnly.Value)
                borrowings = borrowings.Where(b => !b.IsReturned);

            if (request.OverdueOnly.HasValue && request.OverdueOnly.Value)
                borrowings = borrowings.Where(b => !b.IsReturned && b.DueDate < DateTime.UtcNow);

            var borrowingDtos = new List<BorrowingDto>();

            foreach (var borrowing in borrowings)
            {
                var book = await _bookRepository.GetByIdAsync(borrowing.BookId);
                var user = await _userRepository.GetByIdAsync(borrowing.UserId);

                borrowingDtos.Add(new BorrowingDto
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
                });
            }

            return borrowingDtos;
        }

        private string GetBorrowingStatus(Borrowing borrowing)
        {
            if (borrowing.IsReturned) return "Returned";
            if (borrowing.DueDate < DateTime.UtcNow) return "Overdue";
            return "Active";
        }
    }
}
