using System;
using System.Collections.Generic;

namespace ERPNextIntegration.Dtos.ErpNext.SalesInvoice
{
    public class SalesInvoice: IErpNextDto
    {
        public string name { get; set;}
        public string owner { get; set;}
        public DateTimeOffset? creation { get; set;}
        public DateTimeOffset? modified { get; set;}
        public string modified_by { get; set;}
        public string idx { get; set;}
        public int docstatus { get; set;}
        public string title { get; set;}
        public string naming_series { get; set;}
        public string customer { get; set;}
        public string customer_name { get; set;}
        public bool is_pos { get; set;}
        public bool is_consolidated { get; set;}
        public bool is_return { get; set;}
        public bool is_debit_note { get; set;}
        public bool update_billed_amount_in_sales_order { get; set;}
        public string company { get; set;}
        public DateTime? posting_date { get; set;}
        public TimeSpan? posting_time { get; set;}
        public DateTime? due_date { get; set;}
        public string po_no { get; set;}
        public string territory { get; set;}
        public string shipping_address_name { get; set;}
        public string currency { get; set;}
        public decimal? conversion_rate { get; set;}
        public string selling_price_list { get; set;}
        public string price_list_currency { get; set;}
        public decimal? plc_conversion_rate { get; set;}
        public bool ignore_pricing_rule { get; set;}
        public bool update_stock { get; set;}
        public decimal? total_billing_amount { get; set;}
        public decimal? total_qty { get; set;}
        public decimal? base_total { get; set;}
        public decimal? base_net_total { get; set;}
        public string total_net_weight { get; set;}
        public decimal? total { get; set;}
        public decimal? net_total { get; set;}
        public bool exempt_from_sales_tax { get; set;}
        public string tax_category { get; set;}
        public string other_charges_calculation { get; set;}
        public decimal? base_total_taxes_and_charges { get; set;}
        public decimal? total_taxes_and_charges { get; set;}
        public int loyalty_points { get; set;}
        public decimal? loyalty_amount { get; set;}
        public bool redeem_loyalty_points { get; set;}
        public string apply_discount_on { get; set;}
        public decimal? base_discount_amount { get; set;}
        public decimal? additional_discount_percentage { get; set;}
        public decimal? discount_amount { get; set;}
        public decimal? base_grand_total { get; set;}
        public decimal? base_rounding_adjustment { get; set;}
        public decimal? base_rounded_total { get; set;}
        public string base_in_words { get; set;}
        public decimal? grand_total { get; set;}
        public decimal? rounding_adjustment { get; set;}
        public decimal? rounded_total { get; set;}
        public string in_words { get; set;}
        public string total_advance { get; set;}
        public decimal? outstanding_amount { get; set;}
        public bool disable_rounded_total { get; set;}
        public bool allocate_advances_automatically { get; set;}
        public decimal? base_paid_amount { get; set;}
        public decimal? paid_amount { get; set;}
        public string base_change_amount { get; set;}
        public string change_amount { get; set;}
        public string write_off_amount { get; set;}
        public string base_write_off_amount { get; set;}
        public bool write_off_outstanding_amount_automatically { get; set;}
        public bool group_same_items { get; set;}
        public string language { get; set;}
        public bool is_internal_customer { get; set;}
        public string customer_group { get; set;}
        public bool is_discounted { get; set;}
        public string status { get; set;}
        public string debit_to { get; set;}
        public string party_account_currency { get; set;}
        public string is_opening { get; set;}
        public string c_form_applicable { get; set;}
        public string remarks { get; set;}
        public decimal? commission_rate { get; set;}
        public decimal? total_commission { get; set;}
        public string against_income_account { get; set;}
        public string doctype { get; set;}
        public IEnumerable<SalesInvoiceItem> items { get; set; }
        public IEnumerable<SalesTaxesAndCharges> taxes { get; set; }
        public IEnumerable<PaymentSchedule> payment_schedule { get; set; }
    }
}