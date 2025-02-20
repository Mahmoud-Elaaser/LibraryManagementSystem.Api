using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Orders.Queries;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Orders.Handlers
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<GetOrderQueryHandler> _logger;

        public GetOrderQueryHandler(IOrderRepository orderRepository, ILogger<GetOrderQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null) throw new KeyNotFoundException($"Order with ID {request.Id} not found");

            _logger.LogInformation("Retrieved order with ID {OrderId}", order.Id);

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                BookId = order.BookId,
                OrderDate = order.OrderDate,
                ReturnDate = order.ReturnDate,
                Status = order.Status
            };
        }
    }
}
