using Supabase.Gotrue;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mini_ERP.Models
{
    public enum TransactionType
    {
        Sale = 1,
        Refill = 2,
        Adjustment = 3,
        Return = 4,
        WriteOff = 5
    }

    public class InventoryTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        public string? Notes { get; set; }
        public Guid? ReferenceId { get; set; }

        // Add nullable foreign key
        public Guid? ProductId { get; set; }  // This is the new line

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
