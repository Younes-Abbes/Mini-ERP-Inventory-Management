namespace Mini_ERP.Models.Requests.Edit
{
    public class EditInventoryTransactionRequest
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public TransactionType TransactionType { get; set; }

        public decimal Quantity { get; set; }


        public string? Notes { get; set; }
        public int? ReferenceId { get; set; }  // For linking to orders/purchases

        public virtual Product? Product { get; set; }
    }
}
