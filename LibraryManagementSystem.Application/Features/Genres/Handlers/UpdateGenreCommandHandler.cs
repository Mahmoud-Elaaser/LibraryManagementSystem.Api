using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Genres.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Genres.Handlers
{
    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, GenreDto>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<UpdateGenreCommandHandler> _logger;

        public UpdateGenreCommandHandler(
            IGenreRepository genreRepository,
            ILogger<UpdateGenreCommandHandler> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<GenreDto> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Genre with ID {request.Id} not found");

            genre.Name = request.Name;
            genre.Description = request.Description;

            await _genreRepository.UpdateAsync(genre);

            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                Description = genre.Description
            };
        }
    }
}
