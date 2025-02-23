using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Authors.Queries;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Authors.Handlers
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, ILogger<GetAuthorByIdQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {

            var author = await _authorRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Author with ID {request.Id} not found");

            _logger.LogInformation("Retrieved Author with ID {BookId}", request.Id);

            return new AuthorDto
            {
                Id = author.Id,
                Biography = author.Biography,
                BookCount = author.Books.Count,
                DateOfBirth = author.DateOfBirth,
                Name = author.Name,
                Nationality = author.Nationality
            };
        }
    }
}
