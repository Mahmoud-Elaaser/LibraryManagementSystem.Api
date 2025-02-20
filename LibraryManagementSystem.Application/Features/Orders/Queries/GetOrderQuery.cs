using LibraryManagementSystem.Application.DTOs;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Orders.Queries
{
    public class GetOrderQuery : IRequest<OrderDto>
    {
        public int Id { get; set; }
    }
}
