using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public class OrderItemsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<OrderItem> _orderItems;
        public OrderItemsRepository(ApplicationDbContext context)
        {
            _context = context;
            _orderItems = context.orderItems;
        }
        public async Task<IEnumerable<OrderItem>> GetOrderItems()
        {
            return await _orderItems.Include(x => x.Product).ToListAsync();
        }
        public async Task<OrderItem?> GetOrderItem(Guid id)
        {
            return await _orderItems.Include(x => x.Product).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<OrderItem> AddOrderItem(OrderItem orderItem)
        {
            var entry = await _orderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<OrderItem?> UpdateOrderItem(Guid id, OrderItem orderItem)
        {
            var existingOrderItem = await _orderItems.FindAsync(id);
            if (existingOrderItem == null)
            {
                return null;
            }
            existingOrderItem.Quantity = orderItem.Quantity;
            existingOrderItem.Total = orderItem.Total;
            await _context.SaveChangesAsync();
            return existingOrderItem;
        }
        public async Task<OrderItem?> DeleteOrderItem(Guid id)
        {
            var existingOrderItem = await _orderItems.FindAsync(id);
            if (existingOrderItem == null)
            {
                return null;
            }
            _orderItems.Remove(existingOrderItem);
            await _context.SaveChangesAsync();
            return existingOrderItem;
        }
        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
