using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order?> GetOrder(Guid id);
        Task<Order> AddOrder(Order order);
        Task<Order?> UpdateOrder(Guid id, Order order);
        Task<Order?> DeleteOrder(Guid id);

        Task<bool> OrderExists(Guid id);
        void SaveChanges();
    }
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Order> _orders;
        private readonly DbSet<OrderItem> _orderItems;
        public OrdersRepository(ApplicationDbContext context)
        {
            _context = context;
            _orders = context.Order;
            _orderItems = context.orderItems;
        }
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orders.Include(x => x.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
        }
        public async Task<Order?> GetOrder(Guid id)
        {
            return await _orders.Include(x => x.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Order> AddOrder(Order order)
        {
            var entry = await _orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Order?> UpdateOrder(Guid id, Order order)
        {
            var existingOrder = await _orders.FindAsync(id);
            if (existingOrder == null)
            {
                return null;
            }
            existingOrder.Status = order.Status;
            existingOrder.PaymentMethod = order.PaymentMethod;
            existingOrder.ShippingMethod = order.ShippingMethod;
            existingOrder.ShippingAddress = order.ShippingAddress;
            existingOrder.BillingAddress = order.BillingAddress;
            existingOrder.Total = order.Total;
            existingOrder.SubTotal = order.SubTotal;
            existingOrder.DeliveryDate = order.DeliveryDate;
            await _context.SaveChangesAsync();
            return existingOrder;
        }
        public async Task<Order?> DeleteOrder(Guid id)
        {
            var existingOrder = await _orders.FindAsync(id);
            if (existingOrder == null)
            {
                return null;
            }
            _orders.Remove(existingOrder);
            await _context.SaveChangesAsync();
            return existingOrder;
        }
        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> OrderExists(Guid id)
        {
            return await _orders.AnyAsync(e => e.Id == id);
        }
    }
}
