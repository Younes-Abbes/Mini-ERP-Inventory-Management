﻿namespace Mini_ERP.Models.Requests.Edit
{
    public class EditCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
