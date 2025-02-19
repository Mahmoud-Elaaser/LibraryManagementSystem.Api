using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Authors.Queries
{
    public class GetAuthorByIdQuery : IRequest<AuthorDto>
    {
        public int Id { get; set; }
    }
}
