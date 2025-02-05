using System;
using System.ComponentModel.DataAnnotations;

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

        // These dates start as null and are set automatically on status transitions.
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        [Required]
        public ShipmentStatus Status { get; set; }
    }
}
