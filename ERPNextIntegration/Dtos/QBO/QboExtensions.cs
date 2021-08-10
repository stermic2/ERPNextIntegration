using System.Linq;
using ERPNextIntegration.Dtos.ErpNext;
using ERPNextIntegration.Dtos.ErpNext.SalesInvoice;
using ERPNextIntegration.Dtos.ErpNext.Wrapper;
using QuickBooksSharp.Entities;

namespace ERPNextIntegration.Dtos.QBO
{
    public static class QboExtensions
    {
        public static ErpRequest<SalesInvoice> ToErpNext(this Invoice invoice)
        {
            var baseTotal = invoice.Line?.Select(x => x.Amount).Sum();
            return new ErpRequest<SalesInvoice>
            {
                data = new SalesInvoice
                {
                    name = invoice.Id,
                    owner = "mikie@timelabs.com",
                    //creation = invoice.MetaData?.CreateTime,
                    //modified = invoice.MetaData?.LastUpdatedTime,
                    //modified_by = invoice.MetaData?.LastModifiedByRef?.name,
                    //idx = null,
                    docstatus = 0, //enum?
                    title = invoice.CustomerRef?.value + "-" + invoice.Id,
                    naming_series = "ACC-SINV-.YYYY.-",
                    customer = invoice.CustomerRef?.name,
                    customer_name = invoice.CurrencyRef?.name,
                    is_pos = false,
                    is_consolidated = false,
                    is_return = false,
                    is_debit_note = false,
                    update_billed_amount_in_sales_order = false,
                    company = "Time Laboratories",
                    posting_date = invoice.MetaData?.CreateTime?.Date.ToString("yyyy-MM-dd"),
                    posting_time = invoice.MetaData?.CreateTime?.TimeOfDay.ToString("h'h:'m'm:'s's'"),
                    due_date = invoice.DueDate?.ToString("yyyy-MM-dd hh:mm:ss"),
                    po_no = "",
                    territory = "All Territories",
                    shipping_address_name = "",
                    currency = "USD",
                    conversion_rate = 1,
                    selling_price_list = "Standard Selling",
                    price_list_currency = "USD",
                    plc_conversion_rate = 1,
                    ignore_pricing_rule = false,
                    update_stock = false,
                    total_billing_amount = 0,
                    total_qty = invoice.Line?.Select(x => x.SalesItemLineDetail?.Qty).Sum(),
                    base_total = baseTotal,
                    base_net_total = baseTotal,
                    //total_net_weight = null,
                    total = baseTotal,
                    net_total = baseTotal,
                    exempt_from_sales_tax = false,
                    tax_category = "",
                    //other_charges_calculation = null,
                    base_total_taxes_and_charges = invoice.TxnTaxDetail?.TotalTax,
                    total_taxes_and_charges = invoice.TxnTaxDetail?.TotalTax,
                    loyalty_points = 0,
                    loyalty_amount = 0,
                    redeem_loyalty_points = false,
                    apply_discount_on = invoice.ApplyTaxAfterDiscount != null && (bool) invoice.ApplyTaxAfterDiscount ? "Grand Total" : "",
                    base_discount_amount = invoice.DiscountAmt,
                    additional_discount_percentage = invoice.DiscountRate,
                    discount_amount = invoice.DiscountAmt,
                    base_grand_total = baseTotal + invoice.TxnTaxDetail?.TotalTax,
                    base_rounding_adjustment = 0,
                    base_rounded_total = baseTotal + invoice.TxnTaxDetail?.TotalTax,
                    //base_in_words = null,
                    grand_total = invoice.TotalAmt,
                    rounding_adjustment = 0,
                    rounded_total = invoice.TotalAmt,
                    //in_words = null,
                    //total_advance = null,
                    outstanding_amount = invoice.TotalAmt - invoice.Balance,
                    disable_rounded_total = false,
                    allocate_advances_automatically = false,
                    base_paid_amount = invoice.Balance - invoice.TxnTaxDetail?.TotalTax,
                    paid_amount = invoice.Balance,
                    //base_change_amount = null,
                    //change_amount = null,
                    //write_off_amount = null,
                    //base_write_off_amount = null,
                    write_off_outstanding_amount_automatically = false,
                    group_same_items = false,
                    language = "en",
                    is_internal_customer = false,
                    customer_group = "All Customer Groups",
                    is_discounted = false,
                    status = invoice.invoiceStatus,
                    debit_to = "Debtors - TL",
                    party_account_currency = "USD",
                    is_opening = "No",
                    c_form_applicable = "No",
                    remarks = "Qbo Import from webhook",
                    commission_rate = 0,
                    total_commission = 0,
                    against_income_account = "Sales - TL",
                    doctype = "Sales Invoice",
                    items = invoice.Line?
                        .Where(x => x.Id != null)
                        .Select(x => new SalesInvoiceItem
                    {
                        item_code = x.SalesItemLineDetail?.ItemRef?.value,
                        qty = x.SalesItemLineDetail?.Qty
                    }),
                    taxes = invoice.TxnTaxDetail?.TaxLine?.Select(x => new SalesTaxesAndCharges
                    {
                        owner = "mikie@timelabs.com",
                        charge_type = SalesTaxAndChargesType.OnNetTotal,
                        account_head = "ST 6% - TL",
                        rate = x.TaxLineDetail?.TaxPercent,
                        currency = "USD",
                        tax_amount = x.Amount,
                        total = invoice.TotalAmt,
                        tax_amount_after_discount_amount = invoice.TxnTaxDetail?.TotalTax,
                        base_tax_amount = x.Amount,
                        base_total = invoice.TotalAmt,
                        doctype = "Sales Taxes and Charges"
                    }),
                    payment_schedule = null
                }
            };
        }
    }
}