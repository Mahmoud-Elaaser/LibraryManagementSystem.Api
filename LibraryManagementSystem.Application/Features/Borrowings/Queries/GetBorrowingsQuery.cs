using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Borrowings.Queries
{
    public class GetBorrowingsQuery : IRequest<IEnumerable<BorrowingDto>>
    {
        public bool? ActiveOnly { get; set; }
        public bool? OverdueOnly { get; set; }
    }
}
