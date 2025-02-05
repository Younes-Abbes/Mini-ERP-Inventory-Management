using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_ERP.Models
{
    public class PurchaseOrder
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime DeliveryDate { get; set; } = DateTime.UtcNow;

        [Required]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Changed to enum

        [Required]
        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CreditCard; // Changed to enum

        [Required]
        [EnumDataType(typeof(ShippingMethod))]
        public ShippingMethod ShippingMethod { get; set; } = ShippingMethod.Express; // Changed to enum

        [Required]
        [StringLength(255)]
        public string ShippingAddress { get; set; } = "a";

        [Required]
        [StringLength(255)]
        public string BillingAddress { get; set; } = "a";

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; } = 0;

        public Shipment Shipment { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }

        [ForeignKey("supplierId")]
        public Supplier supplier { get; set; }
    }

        public enum OrderStatus
        {
            Pending,
            Processing,
            Shipped,
            Delivered,
            Cancelled
        }

        public enum PaymentMethod
        {
            CreditCard,
            PayPal,
            BankTransfer
        }

        public enum ShippingMethod
        {
            Standard,
            Express,
            Overnight
        }
        
    
}
