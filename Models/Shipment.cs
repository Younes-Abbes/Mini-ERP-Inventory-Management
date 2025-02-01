namespace Mini_ERP.Models
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; } //TODO: make this an enum
    }
}
