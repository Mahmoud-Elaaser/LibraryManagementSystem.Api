using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Books.Queries;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Books.Handlers
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<GetBooksQueryHandler> _logger;

        public GetBooksQueryHandler(
            IBookRepository bookRepository,
            ILogger<GetBooksQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var query = _bookRepository.GetAllAsync();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(searchTerm) ||
                    b.Author.ToLower().Contains(searchTerm) ||
                    b.ISBN.ToLower().Contains(searchTerm));
            }

            // Apply sorting if provided
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                query = request.SortBy.ToLower() switch
                {
                    "title" => query.OrderBy(b => b.Title),
                    "author" => query.OrderBy(b => b.Author),
                    "publicationdate" => query.OrderBy(b => b.PublicationDate),
                    _ => query.OrderBy(b => b.Id)
                };
            }

            // Apply pagination if provided
            if (request.PageNumber.HasValue && request.PageSize.HasValue)
            {
                query = query
                    .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value);
            }

            var books = await query.ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} books", books.Count);

            // Map to DTOs
            var bookDtos = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                PublicationDate = b.PublicationDate
            });

            return bookDtos;
        }
    }
}
