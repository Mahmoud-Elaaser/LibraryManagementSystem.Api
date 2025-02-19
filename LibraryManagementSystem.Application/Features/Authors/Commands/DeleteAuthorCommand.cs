using MediatR;

namespace LibraryManagementSystem.Application.Features.Authors.Commands
{
    public class DeleteAuthorCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
