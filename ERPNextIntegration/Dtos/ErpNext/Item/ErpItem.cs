using System.Collections.Generic;

namespace ERPNextIntegration.Dtos.ErpNext.Item
{
    public class ErpItem
    {
        public string name { get; set; }
        public string owner { get; set; }
        public string creation { get; set; }
        public string modified { get; set; }
        public string modified_by { get; set; }
        public string idx { get; set; }
        public int? docstatus { get; set; }
        public string company { get; set; }
        public string naming_series { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_group { get; set; }
        public bool is_item_from_hub { get; set; }
        public string stock_uom { get; set; }
        public bool? disabled { get; set; }
        public string allow_alternative_item { get; set; }
        public string is_stock_item { get; set; }
        public string include_item_in_manufacturing { get; set; }
        public string opening_stock { get; set; }
        public string valuation_rate { get; set; }
        public string standard_rate { get; set; }
        public string is_fixed_asset { get; set; }
        public string auto_create_assets { get; set; }
        public string over_delivery_receipt_allowance { get; set; }
        public string over_billing_allowance { get; set; }
        public string description { get; set; }
        public string shelf_life_in_days { get; set; }
        public string end_of_life { get; set; }
        public string default_material_request_type { get; set; }
        public string valuation_method { get; set; }
        public string weight_per_unit { get; set; }
        public string has_batch_no { get; set; }
        public string create_new_batch { get; set; }
        public string has_expiry_date { get; set; }
        public string retain_sample { get; set; }
        public string sample_quantity { get; set; }
        public string has_serial_no { get; set; }
        public string has_variants { get; set; }
        public string variant_based_on { get; set; }
        public string is_purchase_item { get; set; }
        public string min_order_qty { get; set; }
        public string safety_stock { get; set; }
        public string lead_time_days { get; set; }
        public string last_purchase_rate { get; set; }
        public string is_customer_provided_item { get; set; }
        public string delivered_by_supplier { get; set; }
        public string sales_uom { get; set; }
        public string is_sales_item { get; set; }
        public string max_discount { get; set; }
        public string enable_deferred_revenue { get; set; }
        public string no_of_months { get; set; }
        public string enable_deferred_expense { get; set; }
        public string no_of_months_exp { get; set; }
        public string inspection_required_before_purchase { get; set; }
        public string inspection_required_before_delivery { get; set; }
        public string is_sub_contracted_item { get; set; }
        public string customer_code { get; set; }
        public string concentration_as_percent { get; set; }
        public string show_in_website { get; set; }
        public string show_variant_in_website { get; set; }
        public string weightage { get; set; }
        public string total_projected_qty { get; set; }
        public string publish_in_hub { get; set; }
        public string synced_with_hub { get; set; }
        public string doctype { get; set; }
        public ICollection<UnitOfMeasurement> uoms { get; set; }
        public ICollection<item_defaults> item_defaults { get; set; }
    }
}