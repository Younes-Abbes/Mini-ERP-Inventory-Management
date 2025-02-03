namespace Mini_ERP.Models.Requests.Create
{
    public class CreateShipmentRequest
    {
        public DateTime ShippedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; } //TODO: make this an enum
    }
}
