using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_ERP.Models
{
    public class PurchaseOrder : Order
    {
        [ForeignKey("customerId")]
        public Customer customer { get; set; }
    }
}
