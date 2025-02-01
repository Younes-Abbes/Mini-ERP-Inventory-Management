namespace Mini_ERP.Models
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
