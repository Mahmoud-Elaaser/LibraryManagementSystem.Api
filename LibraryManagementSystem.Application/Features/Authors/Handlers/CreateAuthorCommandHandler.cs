using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Authors.Commands;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Authors.Handlers
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<CreateAuthorCommandHandler> _logger;

        public CreateAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<CreateAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author
            {
                Name = request.Name,
                Biography = request.Biography,
                DateOfBirth = request.DateOfBirth,
                Nationality = request.Nationality
            };

            var createdAuthor = await _authorRepository.AddAsync(author);
            _logger.LogInformation("Created new author with ID {AuthorId}", createdAuthor.Id);

            return new AuthorDto
            {
                Id = createdAuthor.Id,
                Name = createdAuthor.Name,
                Biography = createdAuthor.Biography,
                DateOfBirth = createdAuthor.DateOfBirth,
                Nationality = createdAuthor.Nationality,
                BookCount = 0
            };
        }
    }
}
