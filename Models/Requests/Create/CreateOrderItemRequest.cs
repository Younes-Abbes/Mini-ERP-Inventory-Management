namespace Mini_ERP.Models.Requests.Create
{
    public class CreateOrderItemRequest
    {
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public int UnitPrice { get; set; } = 0;

        public Guid ProductId { get; set; }
    }
}
