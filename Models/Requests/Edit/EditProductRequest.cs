namespace Mini_ERP.Models.Requests.Edit
{
    public class EditProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int minimumQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Category> Categories { get; set; }

        public virtual Category category { get; set; }
    }
}
