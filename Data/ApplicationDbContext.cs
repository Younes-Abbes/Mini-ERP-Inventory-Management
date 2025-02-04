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
        
        public DbSet<Shipment> shipments { get; set; }
        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<SalesOrder> salesOrders { get; set; }

        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<Mini_ERP.Models.PurchaseOrder> PurchaseOrders { get; set; } = default!;
    }
    
}
