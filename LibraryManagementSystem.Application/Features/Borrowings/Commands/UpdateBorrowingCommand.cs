using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Borrowings.Commands
{
    public class UpdateBorrowingCommand : IRequest<BorrowingDto>
    {
        public int Id { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
