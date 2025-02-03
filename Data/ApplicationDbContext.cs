using Microsoft.EntityFrameworkCore;
using Mini_ERP.Models;

namespace Mini_ERP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> categories { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<PurchaseOrder> purchaseOrders { get; set; }
        public DbSet<Shipment> shipments { get; set; }
        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<SupplierOrder> supplierOrders { get; set; }
        public DbSet<Mini_ERP.Models.Order> Order { get; set; } = default!;
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
    }
    
}
