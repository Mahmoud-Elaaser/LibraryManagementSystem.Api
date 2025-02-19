using LibraryManagementSystem.Application.Features.Authors.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Authors.Handlers
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<DeleteAuthorCommandHandler> _logger;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<DeleteAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Author with ID {request.Id} not found");

            if (author.Books.Any())
            {
                throw new InvalidOperationException("Cannot delete author with existing books");
            }

            await _authorRepository.DeleteAsync(author);
            _logger.LogInformation("Deleted author with ID {AuthorId}", request.Id);

            return true;
        }
    }
}
