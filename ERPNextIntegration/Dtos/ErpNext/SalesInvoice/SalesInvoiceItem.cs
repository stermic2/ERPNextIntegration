using QuickBooksSharp.Entities;

namespace ERPNextIntegration.Dtos.ErpNext.SalesInvoice
{
    public class SalesInvoiceItem
    {
        public string name  { get; set;}
        public string owner  { get; set;}
        public string creation  { get; set;}
        public string modified  { get; set;}
        public string modified_by  { get; set;}
        public string parent  { get; set;}
        public string parentfield  { get; set;}
        public string parenttype  { get; set;}
        public string idx  { get; set;}
        public string docstatus  { get; set;}
        public string item_code  { get; set;}
        public string item_name  { get; set;}
        public string description  { get; set;}
        public string item_group  { get; set;}
        public string image  { get; set;}
        public decimal? qty  { get; set;}
        public string stock_uom  { get; set;}
        public string uom  { get; set;}
        public string conversion_factor  { get; set;}
        public string stock_qty  { get; set;}
        public string price_list_rate  { get; set;}
        public string base_price_list_rate  { get; set;}
        public string margin_type  { get; set;}
        public string margin_rate_or_amount  { get; set;}
        public string rate_with_margin  { get; set;}
        public string discount_percentage  { get; set;}
        public string discount_amount  { get; set;}
        public string base_rate_with_margin  { get; set;}
        public string rate  { get; set;}
        public string amount  { get; set;}
        public string base_rate  { get; set;}
        public string base_amount  { get; set;}
        public string stock_uom_rate  { get; set;}
        public string is_free_item  { get; set;}
        public string net_rate  { get; set;}
        public string net_amount  { get; set;}
        public string base_net_rate  { get; set;}
        public string base_net_amount  { get; set;}
        public string incoming_rate  { get; set;}
        public string delivered_by_supplier  { get; set;}
        public string income_account  { get; set;}
        public string is_fixed_asset  { get; set;}
        public string expense_account  { get; set;}
        public string enable_deferred_revenue  { get; set;}
        public string weight_per_unit  { get; set;}
        public string total_weight  { get; set;}
        public string warehouse  { get; set;}
        public string allow_zero_valuation_rate  { get; set;}
        public string item_tax_rate  { get; set;}
        public string actual_batch_qty  { get; set;}
        public string actual_qty  { get; set;}
        public string delivered_qty  { get; set;}
        public string cost_center  { get; set;}
        public string page_break  { get; set;}
        public string doctype  { get; set;}

        public SalesInvoiceItem()
        {

        }
    }
}