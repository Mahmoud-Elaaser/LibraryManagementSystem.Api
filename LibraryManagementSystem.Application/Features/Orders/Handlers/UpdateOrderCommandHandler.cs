using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Orders.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Orders.Handlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null) throw new KeyNotFoundException($"Order with ID {request.Id} not found");

            order.ReturnDate = request.ReturnDate;
            order.Status = request.Status;

            var updatedOrder = await _orderRepository.UpdateAsync(order);
            _logger.LogInformation("Updated order with ID {OrderId}", updatedOrder.Id);

            return new OrderDto
            {
                Id = updatedOrder.Id,
                UserId = updatedOrder.UserId,
                BookId = updatedOrder.BookId,
                OrderDate = updatedOrder.OrderDate,
                ReturnDate = updatedOrder.ReturnDate,
                Status = updatedOrder.Status
            };
        }
    }
}
