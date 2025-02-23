using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Books.Commands;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Books.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<CreateBookCommandHandler> _logger;

        public CreateBookCommandHandler(IBookRepository bookRepository, ILogger<CreateBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {

            var book = new Book
            {
                Title = request.Title,
                GenreId = request.GenreId,
                AuthorId = request.AuthorId,
                ISBN = request.ISBN,
                PublicationDate = request.PublicationDate,
                IsAvailable = true
            };

            var createdBook = await _bookRepository.AddAsync(book);
            _logger.LogInformation("Created new book with ID {BookId}", createdBook.Id);

            return new BookDto
            {
                Id = createdBook.Id,
                Title = createdBook.Title,
                GenreId = request.GenreId,
                AuthorId = createdBook.AuthorId,
                ISBN = createdBook.ISBN,
                PublicationDate = createdBook.PublicationDate
            };
        }
    }
}
