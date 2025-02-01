using System.ComponentModel;

namespace Mini_ERP.Models
{
    public class Product
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int minimumQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual Category category { get; set; }
    }
}
