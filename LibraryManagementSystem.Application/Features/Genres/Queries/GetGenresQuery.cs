using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Genres.Queries
{
    public class GetGenresQuery : IRequest<IEnumerable<GenreDto>>
    {
    }
}
