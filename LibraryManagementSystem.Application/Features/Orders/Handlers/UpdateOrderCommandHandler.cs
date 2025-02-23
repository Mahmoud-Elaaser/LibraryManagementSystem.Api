using AutoMapper;
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
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(
            IOrderRepository orderRepository,
            ILogger<UpdateOrderCommandHandler> logger,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Order with ID {request.Id} not found");

            _mapper.Map(request, order);
            var updatedOrder = await _orderRepository.UpdateAsync(order);
            _logger.LogInformation("Updated order with ID {OrderId}", updatedOrder.Id);

            return _mapper.Map<OrderDto>(updatedOrder);
        }
    }
}
