using AutoMapper;
using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Genres.Commands;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Mappings
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<CreateGenreCommand, Genre>();
            CreateMap<Genre, GenreDto>();
            CreateMap<UpdateGenreCommand, Genre>();
        }
    }
}
