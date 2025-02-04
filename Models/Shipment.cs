using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_ERP.Models
{
    public enum ShipmentStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Shipment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime ShippedDate { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public ShipmentStatus Status { get; set; }
    }
}
