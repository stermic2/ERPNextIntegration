namespace ERPNextIntegration.Dtos.ErpNext.SalesInvoice
{
    public class PaymentSchedule
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
        public string docstatus { get; set; }
        public string due_date { get; set; }
        public string invoice_portion { get; set; }
        public string discount { get; set; }
        public string payment_amount { get; set; }
        public string outstanding { get; set; }
        public string paid_amount { get; set; }
        public string discounted_amount { get; set; }
        public string base_payment_amount { get; set; }
        public string doctype { get; set; }
    }
}