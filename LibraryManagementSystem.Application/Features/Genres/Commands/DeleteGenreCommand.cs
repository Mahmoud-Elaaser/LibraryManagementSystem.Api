using MediatR;

namespace LibraryManagementSystem.Application.Features.Genres.Commands
{
    public class DeleteGenreCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
