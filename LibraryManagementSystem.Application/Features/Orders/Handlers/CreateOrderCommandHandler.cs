using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Orders.Commands;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Features.Orders.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                UserId = request.UserId,
                BookId = request.BookId,
                OrderDate = DateTime.UtcNow,
                ReturnDate = request.ReturnDate,
                Status = OrderStatus.Pending
            };

            var createdOrder = await _orderRepository.AddAsync(order);
            _logger.LogInformation("Created new order with ID {OrderId}", createdOrder.Id);

            return new OrderDto
            {
                Id = createdOrder.Id,
                UserId = createdOrder.UserId,
                BookId = createdOrder.BookId,
                OrderDate = createdOrder.OrderDate,
                ReturnDate = createdOrder.ReturnDate,
                Status = createdOrder.Status
            };
        }
    }
}
