using AutoMapper;
using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Books.Commands;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();

            CreateMap<CreateBookCommand, Book>()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(_ => true));

            CreateMap<UpdateBookCommand, Book>();
        }
    }
}
