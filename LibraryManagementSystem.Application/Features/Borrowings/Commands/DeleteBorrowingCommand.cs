using MediatR;

namespace LibraryManagementSystem.Application.Features.Borrowings.Commands
{
    public class DeleteBorrowingCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
