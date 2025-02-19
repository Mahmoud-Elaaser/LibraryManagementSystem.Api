using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Genres.Queries;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Genres.Handlers
{
    public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, IEnumerable<GenreDto>>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<GetGenresQueryHandler> _logger;

        public GetGenresQueryHandler(
            IGenreRepository genreRepository,
            ILogger<GetGenresQueryHandler> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<GenreDto>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await _genreRepository.GetAllAsync();

            return genres.Select(genre => new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                Description = genre.Description
            });
        }
    }
}
