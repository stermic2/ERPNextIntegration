using System.Collections.Generic;

namespace ERPNextIntegration.Dtos.ErpNext.Customer
{
    public class ErpAddress : IErpNextDto
    {
        public string name { get; set; }
        public string quickbooks_id { get; set; }
        public string owner { get; set; }
        public string creation { get; set; }
        public string modified { get; set; }
        public string modified_by { get; set; }
        public string idx { get; set; }
        public int docstatus { get; set; }
        public string address_title { get; set; }
        public string address_type { get; set; }
        public string address_line1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public string email_id { get; set; }
        public bool is_primary_address { get; set; }
        public bool is_shipping_address { get; set; }
        public bool disabled { get; set; }
        public bool is_your_company_address { get; set; }
        public string doctype { get; set; }
        public ICollection<ErpAddressLink> links { get; set; }
    }
}