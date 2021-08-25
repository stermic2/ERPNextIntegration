namespace ERPNextIntegration.Dtos.ErpNext.PaymentEntry
{
    public class InvoiceReference
    {
        public string name { get; set; }
        public string owner { get; set; }
        public string creation { get; set; }
        public string modified { get; set; }
        public string modified_by { get; set; }
        public string parent { get; set; }
        public string parentfield { get; set; }
        public string parenttype { get; set; }
        public string idx { get; set; }
        public int docstatus { get; set; }
        public string reference_doctype { get; set; }
        public string reference_name { get; set; }
        public string due_date { get; set; }
        public string total_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string allocated_amount { get; set; }
        public string exchange_rate { get; set; }
        public string doctype { get; set; }
    }
}