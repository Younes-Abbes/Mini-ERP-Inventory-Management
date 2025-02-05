using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public interface IPurchaseOrderRepository
    {
        Task<IEnumerable<PurchaseOrder>> GetOrders();
        Task<PurchaseOrder?> GetOrder(Guid id);
        Task AddPurchaseOrderAsync(PurchaseOrder order);
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersAsync();
        Task<PurchaseOrder> GetPurchaseOrderByIdAsync(Guid id);
        Task UpdatePurchaseOrderAsync(PurchaseOrder order);
        Task DeletePurchaseOrderAsync(int id);




        Task<PurchaseOrder?> UpdateOrder(Guid id, PurchaseOrder order);
        Task<PurchaseOrder?> DeleteOrder(Guid id);

        Task<bool> OrderExists(Guid id);
        void SaveChanges();
    }
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<PurchaseOrder> _orders;
        private readonly DbSet<OrderItem> _orderItems;
        public PurchaseOrderRepository(ApplicationDbContext context)
        {
            _context = context;
            _orders = context.PurchaseOrders;
            _orderItems = context.orderItems;
        }
        public async Task<IEnumerable<PurchaseOrder>> GetOrders()
        {
            return await _orders.Include(x => x.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
        }
        public async Task<PurchaseOrder?> GetOrder(Guid id)
        {
            return await _orders.Include(x => x.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddPurchaseOrderAsync(PurchaseOrder order)
        {
            _context.PurchaseOrders.Add(order);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersAsync()
        {
            return await _context.PurchaseOrders
                .Include(o => o.supplier)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }
        public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(Guid id)
        {
            return await _context.PurchaseOrders
                .Include(o => o.supplier)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task DeletePurchaseOrderAsync(int id)
        {
            var order = await _context.PurchaseOrders.FindAsync(id);
            if (order != null)
            {
                _context.PurchaseOrders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdatePurchaseOrderAsync(PurchaseOrder order)
        {
            _context.PurchaseOrders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<PurchaseOrder?> UpdateOrder(Guid id, PurchaseOrder order)
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
        public async Task<PurchaseOrder?> DeleteOrder(Guid id)
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
