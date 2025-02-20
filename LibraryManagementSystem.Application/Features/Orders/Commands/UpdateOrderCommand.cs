using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Domain.Enums;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<OrderDto>
    {
        public int Id { get; set; }
        public DateTime ReturnDate { get; set; }
        public OrderStatus Status { get; set; }
    }
}
