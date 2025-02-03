namespace Mini_ERP.Models.Requests.Create
{
    public class CreateOrderItemRequest
    {
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public List<Product> Products { get; set; }
        public virtual Product Product { get; set; }
    }
}
