using AutoMapper;
using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Reviews.Commands;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<CreateReviewCommand, Review>()
                .ForMember(dest => dest.ReviewDate, opt => opt.Ignore());

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.Ignore());

            CreateMap<UpdateReviewCommand, Review>();
        }
    }
}
