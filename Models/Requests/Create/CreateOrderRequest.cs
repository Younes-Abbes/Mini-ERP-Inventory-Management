namespace Mini_ERP.Models.Requests.Create
{
    public class CreateOrderRequest
    {
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; } //TODO: make this an enum
        public string PaymentMethod { get; set; } //TODO: make this an enum
        public string ShippingMethod { get; set; } //TODO: make this an enum
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
