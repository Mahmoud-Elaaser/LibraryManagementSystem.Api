using AutoMapper;
using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Borrowings.Commands;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Mappings
{
    public class BorrowingProfile : Profile
    {
        public BorrowingProfile()
        {
            CreateMap<CreateBorrowingCommand, Borrowing>();
            CreateMap<BorrowingDto, Borrowing>().ReverseMap();

            CreateMap<UpdateBorrowingCommand, Borrowing>();
        }
    }
}
