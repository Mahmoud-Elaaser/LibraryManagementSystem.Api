using AutoMapper;
using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Authors.Commands;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Mappings
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<CreateAuthorCommand, Author>();

            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.Books != null ? src.Books.Count : 0));

            CreateMap<UpdateAuthorCommand, Author>();
        }
    }
}
