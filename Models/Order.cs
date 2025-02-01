namespace Mini_ERP.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; } //TODO: make this an enum
        public string PaymentMethod { get; set; } //TODO: make this an enum
        public string ShippingMethod { get; set; } //TODO: make this an enum
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public Shipment Shipment { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
