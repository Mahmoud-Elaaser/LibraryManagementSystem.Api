using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Borrowings.Queries
{
    public class GetUserBorrowingsQuery : IRequest<IEnumerable<BorrowingDto>>
    {
        public int UserId { get; set; }
        public bool? ActiveOnly { get; set; }
    }
}
