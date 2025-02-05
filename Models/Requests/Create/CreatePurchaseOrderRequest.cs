using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mini_ERP.Models.Requests.Create
{
    public class CreatePurchaseOrderRequest
    {
        public DateTime DeliveryDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; } 
        public ShippingMethod ShippingMethod { get; set; } 
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }

        public virtual List<CreateOrderItemRequest> OrderItems { get; set; }

        public string supplierId { get; set; }
        public IEnumerable<SelectListItem> suppliers { get; set; }
    }
}
