using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_ERP.Models
{
    public class SupplierOrder : Order
    {
        [ForeignKey("supplierId")]
        public Supplier Supplier { get; set; }
    }
}
