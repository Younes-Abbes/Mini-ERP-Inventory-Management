using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mini_ERP.Models.Requests.Create
{
    public class CreateProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int minimumQuantity { get; set; }

        public decimal UnitPrice { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public virtual string category { get; set; }

        public bool IsDeleted { get; set; }
    }
}
