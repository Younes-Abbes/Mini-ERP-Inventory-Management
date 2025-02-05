using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public interface ISalesOrderRepository
    {
        Task<IEnumerable<SalesOrder>> GetOrders();
        Task<SalesOrder?> GetOrder(Guid id);
        Task AddSalesOrderAsync(SalesOrder order);
        Task<IEnumerable<SalesOrder>> GetSalesOrdersAsync();
        Task<SalesOrder> GetSalesOrderByIdAsync(Guid id);
        Task UpdateSalesOrderAsync(SalesOrder order);
        Task DeleteSalesOrderAsync(Guid id);




        Task<SalesOrder?> UpdateOrder(Guid id, SalesOrder order);
        Task<SalesOrder?> DeleteOrder(Guid id);

        Task<bool> OrderExists(Guid id);
        void SaveChanges();
    }
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<SalesOrder> _orders;
        private readonly DbSet<OrderItem> _orderItems;
        public SalesOrderRepository(ApplicationDbContext context)
        {
            _context = context;
            _orders = context.salesOrders;
            _orderItems = context.orderItems;
        }
        public async Task<IEnumerable<SalesOrder>> GetOrders()
        {
            return await _orders.Include(x => x.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
        }
        public async Task<SalesOrder?> GetOrder(Guid id)
        {
            return await _orders.Include(x => x.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddSalesOrderAsync(SalesOrder order)
        {
            _context.salesOrders.Add(order);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<SalesOrder>> GetSalesOrdersAsync()
        {
            return await _context.salesOrders
                .Include(o => o.customer)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }
        public async Task<SalesOrder> GetSalesOrderByIdAsync(Guid id)
        {
            return await _context.salesOrders
                .Include(o => o.customer)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task UpdateSalesOrderAsync(SalesOrder order)
        {
            _context.salesOrders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteSalesOrderAsync(Guid id)
        {
            var order = await _context.salesOrders.FindAsync(id);
            if (order != null)
            {
                _context.salesOrders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<SalesOrder?> UpdateOrder(Guid id, SalesOrder order)
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
        public async Task<SalesOrder?> DeleteOrder(Guid id)
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
