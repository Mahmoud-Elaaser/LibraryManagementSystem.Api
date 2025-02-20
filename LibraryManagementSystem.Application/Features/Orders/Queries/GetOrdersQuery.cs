using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Domain.Enums;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Orders.Queries
{
    public class GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
        public OrderStatus? Status { get; set; }
        public int? UserId { get; set; }
    }
}
