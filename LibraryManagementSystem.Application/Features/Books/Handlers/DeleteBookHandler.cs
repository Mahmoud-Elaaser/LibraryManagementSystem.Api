using LibraryManagementSystem.Application.Features.Books.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Books.Handlers
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<DeleteBookCommandHandler> _logger;

        public DeleteBookCommandHandler(IBookRepository bookRepository, ILogger<DeleteBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Book with ID {request.Id} not found");

            await _bookRepository.DeleteAsync(book);
            _logger.LogInformation("Deleted book with ID {BookId}", request.Id);

            return true;
        }
    }
}
