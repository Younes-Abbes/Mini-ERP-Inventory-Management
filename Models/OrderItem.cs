namespace Mini_ERP.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public virtual Product Product { get; set; }

    }
}
