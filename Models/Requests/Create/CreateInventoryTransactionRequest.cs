using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mini_ERP.Models.Requests.Create
{
    public class CreateInventoryTransactionRequest
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public TransactionType TransactionType { get; set; }

        public decimal Quantity { get; set; }


        public string? Notes { get; set; }
        public Guid? ReferenceId { get; set; }  // For linking to orders/purchases

        public virtual string? Product { get; set; }
        public IEnumerable<SelectListItem> products { get; set; }
    }
}
