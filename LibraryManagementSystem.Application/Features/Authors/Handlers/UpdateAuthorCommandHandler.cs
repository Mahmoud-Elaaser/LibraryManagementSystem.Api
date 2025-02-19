using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Authors.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Authors.Handlers
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<UpdateAuthorCommandHandler> _logger;

        public UpdateAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<UpdateAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<AuthorDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Author with ID {request.Id} not found");

            author.Name = request.Name;
            author.Biography = request.Biography;
            author.DateOfBirth = request.DateOfBirth;
            author.Nationality = request.Nationality;

            await _authorRepository.UpdateAsync(author);
            _logger.LogInformation("Updated author with ID {AuthorId}", author.Id);

            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                DateOfBirth = author.DateOfBirth,
                Nationality = author.Nationality,
                BookCount = author.Books.Count
            };
        }
    }
}
