using ERPNextIntegration.Dtos.ErpNext;
using ERPNextIntegration.Dtos.ErpNext.Supplier;
using ERPNextIntegration.Dtos.ErpNext.Wrapper;
using QuickBooksSharp.Entities;

namespace ERPNextIntegration.Dtos.QBO.QboExtensions
{
    public static class QboPurchaseByVendorExtensions
    {
        public static IErpNextDto ToErpSupplier(this Vendor vendor)
        {
            return new ErpSupplier
            {
                name = vendor.DisplayName,
                quickbooks_id = vendor.Id,
                //owner = null,
                //creation = null,
                //modified = null,
                //modified_by = null,
                //idx = null,
                docstatus = 0,
                company = "Time Laboratories",
                //naming_series = null,
                supplier_name = vendor.DisplayName,
                country = vendor.TaxCountry,
                irs_1099 = false,
                is_transporter = null,
                is_internal_supplier = null,
                supplier_group = "All Supplier Groups",
                supplier_type = null,
                allow_purchase_invoice_creation_without_purchase_order = null,
                allow_purchase_invoice_creation_without_purchase_receipt = null,
                disabled = null,
                warn_rfqs = null,
                warn_pos = null,
                prevent_rfqs = null,
                prevent_pos = null,
                on_hold = null,
                hold_type = null,
                language = null,
                is_frozen = null,
                doctype = null,
                companies = null,
                accounts = null
            };
        }
    }
}