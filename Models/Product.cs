using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Mini_ERP.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int minimumQuantity { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public virtual Category category { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<InventoryTransaction>? InventoryTransactions { get; set; }

    }
}
