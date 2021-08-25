using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicCQ.Requests.Queries.FindQuery;
using ERPNextIntegration.Dtos.ErpNext.PaymentEntry;
using ERPNextIntegration.Dtos.ErpNext.SalesInvoice;
using ERPNextIntegration.IntegrationRelationships;
using MediatR;
using QuickBooksSharp.Entities;

namespace ERPNextIntegration.Dtos.QBO.QboExtensions
{
    public static class QboPaymentExtensions
    {
        public static async Task<ErpPaymentEntry> ToErpPaymentEntry(this Payment payment, IMediator dispatcher)
        {
            IEnumerable<Task<SalesInvoiceRelationship>> relatedSalesInvoicesQueries =
                payment.Line?.FirstOrDefault()?.LinkedTxn?.Select(async x =>
                    (await dispatcher.Send(new FindQuery<SalesInvoiceRelationship>(x.TxnId))).Records.FirstOrDefault());
            IEnumerable<SalesInvoiceRelationship> relatedSalesInvoices = await Task.WhenAll(relatedSalesInvoicesQueries);
            // To Do: When no relatedSalesInvoices are found throw a more specific error than null reference
            return new ErpPaymentEntry
            {
                name = payment.Id,
                //owner = null,
                //creation = null,
                //modified = null,
                //modified_by = null,
                //idx = null,
                docstatus = 0,
                naming_series = "ACC-PAY-.YYYY.-",
                payment_type = payment.PaymentType?.ToString() == "Expense" ? "Expense" : "Receive",
                //payment_order_status = null,
                posting_date = payment.TxnDate?.ToString("yyyy-MM-dd hh:mm:ss"),
                company = "Time Laboratories",
                mode_of_payment = payment.PaymentType?.ToString() ?? "Unspecified",
                party_type = "Customer",
                party = payment.CustomerRef?.name,
                party_name = payment.CustomerRef?.name,
                //contact_email = null,
                //party_balance = null,
                paid_from = "Debtors - TL",
                paid_from_account_currency = "USD",
                //paid_from_account_balance = null,
                paid_to = payment.PaymentType + " - TL",
                paid_to_account_currency = "USD",
                //paid_to_account_balance = null,
                //paid_amount = null,
                paid_amount_after_tax = payment.TotalAmt,
                source_exchange_rate = 1,
                //base_paid_amount = null,
                base_paid_amount_after_tax = payment.TotalAmt,
                //received_amount = null,
                received_amount_after_tax = payment.TotalAmt,
                target_exchange_rate = 1,
                //base_received_amount = null,
                base_received_amount_after_tax = payment.TotalAmt,
                //total_allocated_amount = null,
                base_total_allocated_amount = payment.TotalAmt,
                unallocated_amount = payment.UnappliedAmt,
                difference_amount = payment.UnappliedAmt,
                apply_tax_withholding_amount = false,
                base_total_taxes_and_charges = payment.TxnTaxDetail?.TotalTax,
                total_taxes_and_charges = payment.TxnTaxDetail?.TotalTax,
                //status = null,
                custom_remarks = false,
                //remarks = null,
                title = payment.CustomerRef?.name,
                doctype = "Payment Entry",
                references = relatedSalesInvoices.Select(x => new InvoiceReference
                {
                    //name = null,
                    //owner = null,
                    //creation = null,
                    //modified = null,
                    //modified_by = null,
                    parent = payment.Id,
                    parentfield = "references",
                    parenttype = "Payment Entry",
                    //idx = null,
                    docstatus = 0,
                    reference_doctype = "Sales Invoice",
                    reference_name = x.name,
                    //due_date = null,
                    //total_amount = null,
                    //outstanding_amount = null,
                    //allocated_amount = null,
                    //exchange_rate = null,
                    doctype = "Payment Entry Reference"
                })
            };

        }
    }
}