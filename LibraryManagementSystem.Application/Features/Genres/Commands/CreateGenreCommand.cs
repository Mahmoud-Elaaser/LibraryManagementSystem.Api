using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Genres.Commands
{
    public class CreateGenreCommand : IRequest<GenreDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
