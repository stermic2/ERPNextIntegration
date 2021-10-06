namespace ERPNextIntegration.Dtos.ErpNext.Supplier
{
    public class ErpSupplier : IErpNextDto
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
        public string supplier_name { get; set; }
        public string country { get; set; }
        public bool irs_1099 { get; set; }
        public string is_transporter { get; set; }
        public string is_internal_supplier { get; set; }
        public string supplier_group { get; set; }
        public string supplier_type { get; set; }
        public string allow_purchase_invoice_creation_without_purchase_order { get; set; }
        public string allow_purchase_invoice_creation_without_purchase_receipt { get; set; }
        public string disabled { get; set; }
        public string warn_rfqs { get; set; }
        public string warn_pos { get; set; }
        public string prevent_rfqs { get; set; }
        public string prevent_pos { get; set; }
        public string on_hold { get; set; }
        public string hold_type { get; set; }
        public string language { get; set; }
        public string is_frozen { get; set; }
        public string doctype { get; set; }
        public string companies { get; set; }
        public string accounts { get; set; }
    }
}