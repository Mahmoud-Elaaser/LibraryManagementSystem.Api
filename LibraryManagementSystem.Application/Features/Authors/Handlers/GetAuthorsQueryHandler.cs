using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Authors.Queries;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Authors.Handlers
{
    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<AuthorDto>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAuthorsQueryHandler> _logger;

        public GetAuthorsQueryHandler(IAuthorRepository authorRepository, ILogger<GetAuthorsQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            var query = _authorRepository.GetAllAsync();//.Include(a => a.Books);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(a =>
                    a.Name.ToLower().Contains(searchTerm) ||
                    a.Nationality.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(request.SortBy))
            {
                query = request.SortBy.ToLower() switch
                {
                    "name" => query.OrderBy(a => a.Name),
                    "nationality" => query.OrderBy(a => a.Nationality),
                    "dateofbirth" => query.OrderBy(a => a.DateOfBirth),
                    "bookcount" => query.OrderBy(a => a.Books.Count),
                    _ => query.OrderBy(a => a.Id)
                };
            }

            if (request.PageNumber.HasValue && request.PageSize.HasValue)
            {
                query = query
                    .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value);
            }

            var authors = await query.ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} authors", authors.Count);

            return authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name,
                Biography = a.Biography,
                DateOfBirth = a.DateOfBirth,
                Nationality = a.Nationality,
                BookCount = a.Books.Count
            });
        }
    }
}
