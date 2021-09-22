using System.Collections.Generic;

namespace ERPNextIntegration.Dtos.ErpNext.PaymentEntry
{
    public class ErpPaymentEntry
    {
        public string name { get; set; }
        public string owner { get; set; }
        public string creation { get; set; }
        public string modified { get; set; }
        public string modified_by { get; set; }
        public string idx { get; set; }
        public int docstatus { get; set; }
        public string naming_series { get; set; }
        public string payment_type { get; set; }
        public string payment_order_status { get; set; }
        public string posting_date { get; set; }
        public string company { get; set; }
        public string mode_of_payment { get; set; }
        public string party_type { get; set; }
        public string party { get; set; }
        public string party_name { get; set; }
        public string contact_email { get; set; }
        public string party_balance { get; set; }
        public string paid_from { get; set; }
        public string paid_from_account_currency { get; set; }
        public string paid_from_account_balance { get; set; }
        public string paid_to { get; set; }
        public string paid_to_account_currency { get; set; }
        public string paid_to_account_balance { get; set; }
        public decimal? paid_amount { get; set; }
        public decimal? paid_amount_after_tax { get; set; }
        public decimal? source_exchange_rate { get; set; }
        public string base_paid_amount { get; set; }
        public decimal? base_paid_amount_after_tax { get; set; }
        public decimal? received_amount { get; set; }
        public decimal? received_amount_after_tax { get; set; }
        public decimal? target_exchange_rate { get; set; }
        public string base_received_amount { get; set; }
        public decimal? base_received_amount_after_tax { get; set; }
        public string total_allocated_amount { get; set; }
        public decimal? base_total_allocated_amount { get; set; }
        public decimal? unallocated_amount { get; set; }
        public decimal? difference_amount { get; set; }
        public bool apply_tax_withholding_amount { get; set; }
        public decimal? base_total_taxes_and_charges { get; set; }
        public decimal? total_taxes_and_charges { get; set; }
        public string status { get; set; }
        public bool custom_remarks { get; set; }
        public string remarks { get; set; }
        public string title { get; set; }
        public string doctype { get; set; }
        public IEnumerable<InvoiceReference> references { get; set; }
        //"taxes { get; set; }
        //"deductions { get; set; }
    }
}