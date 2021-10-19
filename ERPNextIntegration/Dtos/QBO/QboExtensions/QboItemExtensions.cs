using System.Collections.Generic;
using ERPNextIntegration.Dtos.ErpNext;
using ERPNextIntegration.Dtos.ErpNext.Item;
using ERPNextIntegration.Dtos.ErpNext.Wrapper;
using QuickBooksSharp.Entities;

namespace ERPNextIntegration.Dtos.QBO.QboExtensions
{
    public static class QboItemExtensions
    {
        public static IErpNextDto ToErpItem(this Item qboItem)
        {
            return new ErpItem
            {
                //name = qboItem.Id,
                quickbooks_id = qboItem.Id,
                owner = "mikie@timelabs.com",
                //creation = null,
                //modified = null,
                //modified_by = null,
                //idx = null,
                docstatus = 0,
                company = "Time Laboratories",
                //naming_series = null,
                item_code = qboItem.Sku,
                item_name = qboItem.FullyQualifiedName,
                //item_group = null,
                is_item_from_hub = false,
                //stock_uom = null,
                disabled = !qboItem.Active,
                //allow_alternative_item = null,
                //is_stock_item = null,
                include_item_in_manufacturing = null,
                //opening_stock = null,
                //valuation_rate = null,
                //standard_rate = null,
                //is_fixed_asset = null,
                //auto_create_assets = null,
                //over_delivery_receipt_allowance = null,
                //over_billing_allowance = null,
                description = qboItem.Description,
                shelf_life_in_days = null,
                //end_of_life = null,
                //default_material_request_type = null,
                //valuation_method = null,
                //weight_per_unit = null,
                //has_batch_no = null,
                //create_new_batch = null,
                //has_expiry_date = null,
                //retain_sample = null,
                //sample_quantity = null,
                //has_serial_no = null,
                //has_variants = null,
                //variant_based_on = null,
                //is_purchase_item = null,
                //min_order_qty = null,
                //safety_stock = null,
                //lead_time_days = null,
                //last_purchase_rate = null,
                //is_customer_provided_item = null,
                //delivered_by_supplier = null,
                //sales_uom = null,
                //is_sales_item = null,
                //max_discount = null,
                //enable_deferred_revenue = null,
                //no_of_months = null,
                //enable_deferred_expense = null,
                //no_of_months_exp = null,
                //inspection_required_before_purchase = null,
                //inspection_required_before_delivery = null,
                //is_sub_contracted_item = null,
                //customer_code = null,
                //concentration_as_percent = null,
                //show_in_website = null,
                //show_variant_in_website = null,
                //weightage = null,
                //total_projected_qty = null,
                //publish_in_hub = null,
                //synced_with_hub = null,
                //doctype = null,
                uoms = new List<UnitOfMeasurement>
                {
                    new UnitOfMeasurement
                    {
                        //name = null,
                        //owner = null,
                        //creation = null,
                        //modified = null,
                        //modified_by = null,
                        parent = qboItem.Sku,
                        parentfield = "uoms",
                        parenttype = "Item",
                        //idx = null,
                        docstatus = 0,
                        uom = "Nos",
                        conversion_factor = 1,
                        doctype = "UOM Conversion Detail"
                    }
                },
                item_defaults = new List<item_defaults>
                {
                    new item_defaults
                    {
                        //name = null,
                        //owner = null,
                        //creation = null,
                        //modified = null,
                        //modified_by = null,
                        //parent = null,
                        //parentfield = null,
                        //parenttype = null,
                        //idx = null,
                        docstatus = 0,
                        company = "Time Laboratories",
                        default_warehouse = "Stores - TL",
                        default_price_list = "Standard Selling",
                        income_account = "Sales - TL",
                        doctype = "Item Default"
                    }
                }
            };
        }
    }
}
