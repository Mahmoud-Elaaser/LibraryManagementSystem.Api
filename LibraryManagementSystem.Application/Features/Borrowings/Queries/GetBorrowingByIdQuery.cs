using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Borrowings.Queries
{
    public class GetBorrowingByIdQuery : IRequest<BorrowingDto>
    {
        public int Id { get; set; }
    }
}
