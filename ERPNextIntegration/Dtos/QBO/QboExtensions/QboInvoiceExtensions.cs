using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicCQ.Interfaces;
using DynamicCQ.RequestEncapsulation;
using DynamicCQ.Requests.Commands.AddCommand;
using DynamicCQ.Requests.Commands.UpdateCommand;
using DynamicCQ.Requests.Queries.FindQuery;
using ERPNextIntegration.Dtos.ErpNext;
using ERPNextIntegration.Dtos.ErpNext.SalesInvoice;
using ERPNextIntegration.Dtos.ErpNext.Wrapper;
using MediatR;
using QuickBooksSharp.Entities;

namespace ERPNextIntegration.Dtos.QBO.QboExtensions
{
    public static class QboInvoiceExtensions
    {
        public static IErpNextDto ToErpInvoice(this Invoice invoice)
        {
            var salesInvoice = invoice.ToErpInvoiceFromSalesTransaction();
            salesInvoice.status = invoice.invoiceStatus;
            return salesInvoice;
        }
        public static IErpNextDto ToErpInvoice(this SalesReceipt salesReceipt)
        {
            var salesInvoice = salesReceipt.ToErpInvoiceFromSalesTransaction();
            return salesInvoice;
        }

        private static SalesInvoice ToErpInvoiceFromSalesTransaction(this SalesTransaction salesTransaction)
        {
            var baseTotal = salesTransaction.Line?.Select(x => x.Amount).Sum();
            return new SalesInvoice
            {
                name = salesTransaction.Id,
                quickbooks_id = salesTransaction.Id,
                owner = "mikie@timelabs.com",
                //creation = invoice.MetaData?.CreateTime,
                //modified = invoice.MetaData?.LastUpdatedTime,
                //modified_by = invoice.MetaData?.LastModifiedByRef?.name,
                //idx = null,
                docstatus = 0, //enum?
                title = salesTransaction.CustomerRef?.value + "-" + salesTransaction.Id,
                naming_series = "ACC-SINV-.YYYY.-",
                customer = "Test Customer",//invoice.CustomerRef?.name,
                customer_name = salesTransaction.CurrencyRef?.name,
                is_pos = false,
                is_consolidated = false,
                is_return = false,
                is_debit_note = false,
                update_billed_amount_in_sales_order = false,
                company = "Time Laboratories",
                posting_date = salesTransaction.MetaData?.CreateTime?.Date.ToString("yyyy-MM-dd"),
                posting_time = salesTransaction.MetaData?.CreateTime?.TimeOfDay.ToString("h'h:'m'm:'s's'"),
                due_date = salesTransaction.DueDate?.ToString("yyyy-MM-dd hh:mm:ss"),
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
                total_qty = salesTransaction.Line?.Select(x => x.SalesItemLineDetail?.Qty).Sum(),
                base_total = baseTotal,
                base_net_total = baseTotal,
                //total_net_weight = null,
                total = baseTotal,
                net_total = baseTotal,
                exempt_from_sales_tax = false,
                tax_category = "",
                //other_charges_calculation = null,
                base_total_taxes_and_charges = salesTransaction.TxnTaxDetail?.TotalTax,
                total_taxes_and_charges = salesTransaction.TxnTaxDetail?.TotalTax,
                loyalty_points = 0,
                loyalty_amount = 0,
                redeem_loyalty_points = false,
                apply_discount_on = salesTransaction.ApplyTaxAfterDiscount != null && (bool) salesTransaction.ApplyTaxAfterDiscount ? "Grand Total" : "",
                base_discount_amount = salesTransaction.DiscountAmt,
                additional_discount_percentage = salesTransaction.DiscountRate,
                discount_amount = salesTransaction.DiscountAmt,
                base_grand_total = baseTotal + salesTransaction.TxnTaxDetail?.TotalTax,
                base_rounding_adjustment = 0,
                base_rounded_total = baseTotal + salesTransaction.TxnTaxDetail?.TotalTax,
                //base_in_words = null,
                grand_total = salesTransaction.TotalAmt,
                rounding_adjustment = 0,
                rounded_total = salesTransaction.TotalAmt,
                //in_words = null,
                //total_advance = null,
                outstanding_amount = salesTransaction.TotalAmt - salesTransaction.Balance,
                disable_rounded_total = false,
                allocate_advances_automatically = false,
                base_paid_amount = salesTransaction.Balance - salesTransaction.TxnTaxDetail?.TotalTax,
                paid_amount = salesTransaction.Balance,
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
                //status = "",
                debit_to = "Debtors - TL",
                party_account_currency = "USD",
                is_opening = "No",
                c_form_applicable = "No",
                remarks = "Qbo Import from webhook",
                commission_rate = 0,
                total_commission = 0,
                against_income_account = "Sales - TL",
                doctype = "Sales Invoice",
                items = salesTransaction.Line?
                    .Where(x => x.Id != null)
                    .Select(x => new SalesInvoiceItem
                {
                    item_code = "MTBAH",//x.SalesItemLineDetail?.ItemRef?.value,
                    qty = x.SalesItemLineDetail?.Qty
                }),
                taxes = salesTransaction.TxnTaxDetail?.TaxLine?.Select(x => new SalesTaxesAndCharges
                {
                    owner = "mikie@timelabs.com",
                    charge_type = SalesTaxAndChargesType.OnNetTotal,
                    description = x.Description,
                    account_head = "ST 6% - TL",
                    rate = x.TaxLineDetail?.TaxPercent,
                    currency = "USD",
                    tax_amount = x.Amount,
                    total = salesTransaction.TotalAmt,
                    tax_amount_after_discount_amount = salesTransaction.TxnTaxDetail?.TotalTax,
                    base_tax_amount = x.Amount,
                    base_total = salesTransaction.TotalAmt,
                    doctype = "Sales Taxes and Charges"
                }),
                payment_schedule = null
            };
        }
    }
}