using ERPNextIntegration.Dtos.ErpNext.Item;
using QuickBooksSharp.Entities;

namespace ERPNextIntegration.Dtos.QBO.QboExtensions
{
    public static class QboItemExtensions
    {
        public static ErpItem ToErpItem(this Item qboItem)
        {
            return new ErpItem
            {
                name = null,
                owner = null,
                creation = null,
                modified = null,
                modified_by = null,
                idx = null,
                docstatus = null,
                company = null,
                naming_series = null,
                item_code = null,
                item_name = null,
                item_group = null,
                is_item_from_hub = null,
                stock_uom = null,
                disabled = null,
                allow_alternative_item = null,
                is_stock_item = null,
                include_item_in_manufacturing = null,
                opening_stock = null,
                valuation_rate = null,
                standard_rate = null,
                is_fixed_asset = null,
                auto_create_assets = null,
                over_delivery_receipt_allowance = null,
                over_billing_allowance = null,
                description = null,
                shelf_life_in_days = null,
                end_of_life = null,
                default_material_request_type = null,
                valuation_method = null,
                weight_per_unit = null,
                has_batch_no = null,
                create_new_batch = null,
                has_expiry_date = null,
                retain_sample = null,
                sample_quantity = null,
                has_serial_no = null,
                has_variants = null,
                variant_based_on = null,
                is_purchase_item = null,
                min_order_qty = null,
                safety_stock = null,
                lead_time_days = null,
                last_purchase_rate = null,
                is_customer_provided_item = null,
                delivered_by_supplier = null,
                sales_uom = null,
                is_sales_item = null,
                max_discount = null,
                enable_deferred_revenue = null,
                no_of_months = null,
                enable_deferred_expense = null,
                no_of_months_exp = null,
                inspection_required_before_purchase = null,
                inspection_required_before_delivery = null,
                is_sub_contracted_item = null,
                customer_code = null,
                concentration_as_percent = null,
                show_in_website = null,
                show_variant_in_website = null,
                weightage = null,
                total_projected_qty = null,
                publish_in_hub = null,
                synced_with_hub = null,
                doctype = null
            };
        }
    }
}