using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Genres.Queries
{
    public class GetGenreByIdQuery : IRequest<GenreDto>
    {
        public int Id { get; set; }
    }
}
