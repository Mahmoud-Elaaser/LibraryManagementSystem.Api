using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Orders.Queries;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Orders.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<GetOrdersQueryHandler> _logger;

        public GetOrdersQueryHandler(IOrderRepository orderRepository, ILogger<GetOrdersQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync();

            if (request.Status.HasValue)
                orders = orders.Where(o => o.Status == request.Status);

            if (request.UserId.HasValue)
                orders = orders.Where(o => o.UserId == request.UserId);

            _logger.LogInformation("Retrieved {Count} orders", orders.Count());

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                BookId = o.BookId,
                OrderDate = o.OrderDate,
                ReturnDate = o.ReturnDate,
                Status = o.Status
            });
        }
    }
}
