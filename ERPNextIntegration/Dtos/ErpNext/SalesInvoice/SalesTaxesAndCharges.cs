using QuickBooksSharp.Entities;

namespace ERPNextIntegration.Dtos.ErpNext.SalesInvoice
{
    public class SalesTaxesAndCharges
    {
        public string name { get; set; }
        public string owner { get; set; }
        public string creation { get; set; }
        public string modified { get; set; }
        public string modified_by { get; set; }
        public string parent { get; set; }
        public string parentfield { get; set; }
        public string parenttype { get; set; }
        public string idx { get; set; }
        public string docstatus { get; set; }
        public string charge_type { get; set; }
        public string account_head { get; set; }
        public string description { get; set; }
        public string included_in_print_rate { get; set; }
        public string included_in_paid_amount { get; set; }
        public string cost_center { get; set; }
        public decimal? rate { get; set; }
        public string currency { get; set; }
        public decimal? tax_amount { get; set; }
        public decimal? total { get; set; }
        public decimal? tax_amount_after_discount_amount { get; set; }
        public decimal? base_tax_amount { get; set; }
        public decimal? base_total { get; set; }
        public string base_tax_amount_after_discount_amount { get; set; }
        public string item_wise_tax_detail { get; set; }
        public string doctype { get; set; }
    }
}