namespace Mini_ERP.Models.Requests.Edit
{
    public class EditInventoryTransactionRequest
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public TransactionType TransactionType { get; set; }

        public decimal Quantity { get; set; }


        public string? Notes { get; set; }
        public Guid? ReferenceId { get; set; }  // For linking to orders/purchases

        public virtual Product? Product { get; set; }
    }
}
