using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_ERP.Models.Requests.Create
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
