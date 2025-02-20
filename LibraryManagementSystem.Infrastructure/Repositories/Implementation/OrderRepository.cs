using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using LibraryManagementSystem.Infrastructure.Context;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Infrastructure.Repositories.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ApplicationDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Order> AddAsync(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully added order with ID {OrderId}", order.Id);
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding order");
                throw;
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", id);
                    return null;
                }

                _logger.LogInformation("Successfully retrieved order with ID {OrderId}", id);
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving order with ID {OrderId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                _logger.LogInformation("Successfully retrieved {Count} orders", orders.Count);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all orders");
                throw;
            }
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            try
            {
                var existingOrder = await _context.Orders.FindAsync(order.Id);
                if (existingOrder == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found for update", order.Id);
                    return null;
                }


                existingOrder.ReturnDate = order.ReturnDate;
                existingOrder.Status = order.Status;

                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated order with ID {OrderId}", order.Id);
                return existingOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating order with ID {OrderId}", order.Id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found for deletion", id);
                    return;
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted order with ID {OrderId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting order with ID {OrderId}", id);
                throw;
            }
        }


        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} orders for user {UserId}", orders.Count, userId);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving orders for user {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.Status == status)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} orders with status {Status}", orders.Count, status);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving orders with status {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetOverdueOrdersAsync()
        {
            try
            {
                var currentDate = DateTime.UtcNow;
                var orders = await _context.Orders
                    .Where(o => o.ReturnDate < currentDate && o.Status == OrderStatus.Active)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} overdue orders", orders.Count);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving overdue orders");
                throw;
            }
        }
    }
}
