using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Genres.Queries;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Genres.Handlers
{
    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreDto>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<GetGenreByIdQueryHandler> _logger;

        public GetGenreByIdQueryHandler(
            IGenreRepository genreRepository,
            ILogger<GetGenreByIdQueryHandler> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Genre with ID {request.Id} not found");

            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                Description = genre.Description
            };
        }
    }
}
