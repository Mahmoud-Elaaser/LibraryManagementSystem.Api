using LibraryManagementSystem.Application.Features.Genres.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Genres.Handlers
{
    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, bool>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<DeleteGenreCommandHandler> _logger;

        public DeleteGenreCommandHandler(
            IGenreRepository genreRepository,
            IBookRepository bookRepository,
            ILogger<DeleteGenreCommandHandler> logger)
        {
            _genreRepository = genreRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Genre with ID {request.Id} not found");

            // Check if there are any books associated with this genre
            var booksWithGenre = await _bookRepository.GetByIdAsync(request.Id);
            if (booksWithGenre != null)
            {
                throw new InvalidOperationException($"Cannot delete genre with ID {request.Id} as it is associated with existing books");
            }

            await _genreRepository.DeleteAsync(genre);
            return true;
        }
    }
}
