namespace ERPNextIntegration.Dtos.ErpNext.Customer
{
    public class ErpCustomer
    {
        public string name { get; set; }
        public string owner { get; set; }
        public string creation { get; set; }
        public string modified { get; set; }
        public string modified_by { get; set; }
        public string idx { get; set; }
        public int docstatus { get; set; }
        public string company { get; set; }
        public string naming_series { get; set; }
        public string customer_name { get; set; }
        public string customer_type { get; set; }
        public string customer_group { get; set; }
        public string territory { get; set; }
        public bool so_required { get; set; }
        public bool dn_required { get; set; }
        public bool disabled { get; set; }
        public bool is_internal_customer { get; set; }
        public bool exempt_from_sales_tax { get; set; }
        public string language { get; set; }
        public bool is_frozen { get; set; }
        public string default_commission_rate { get; set; }
        public string doctype { get; set; }
        public string companies { get; set; }
        public string accounts { get; set; }
        public string credit_limits { get; set; }
        public string sales_team { get; set; }
    }
}