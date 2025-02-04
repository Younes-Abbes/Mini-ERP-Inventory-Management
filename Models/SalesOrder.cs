using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_ERP.Models
{
    public class SalesOrder
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime createdAt { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; } // Changed to enum

        [Required]
        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get; set; } // Changed to enum

        [Required]
        [EnumDataType(typeof(ShippingMethod))]
        public ShippingMethod ShippingMethod { get; set; } // Changed to enum

        [Required]
        [StringLength(255)]
        public string ShippingAddress { get; set; }

        [Required]
        [StringLength(255)]
        public string BillingAddress { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        public Shipment Shipment { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
        [ForeignKey("customerId")]
        public Customer customer { get; set; }
    }
    
}
