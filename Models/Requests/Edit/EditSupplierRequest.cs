namespace Mini_ERP.Models.Requests.Edit
{
    public class EditSupplierRequest
    {
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
