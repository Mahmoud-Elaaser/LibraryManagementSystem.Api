using AutoMapper;
using LibraryManagementSystem.Application.DTOs.Auth;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.MembershipDate, opt => opt.Ignore());

        }
    }
}
