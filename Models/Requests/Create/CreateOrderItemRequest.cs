namespace Mini_ERP.Models.Requests.Create
{
    public class CreateOrderItemRequest
    {
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public Guid ProductId { get; set; }
    }
}
