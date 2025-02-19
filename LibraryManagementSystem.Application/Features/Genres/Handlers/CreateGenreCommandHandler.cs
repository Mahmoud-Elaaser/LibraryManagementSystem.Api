using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Genres.Commands;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Genres.Handlers
{
    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreDto>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<CreateGenreCommandHandler> _logger;

        public CreateGenreCommandHandler(
            IGenreRepository genreRepository,
            ILogger<CreateGenreCommandHandler> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<GenreDto> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = new Genre
            {
                Name = request.Name,
                Description = request.Description
            };

            var createdGenre = await _genreRepository.AddAsync(genre);

            return new GenreDto
            {
                Id = createdGenre.Id,
                Name = createdGenre.Name,
                Description = createdGenre.Description
            };
        }
    }
}
