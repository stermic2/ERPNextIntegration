namespace ERPNextIntegration.Dtos.ErpNext.Item
{
    public class UnitOfMeasurement
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
        public string uom { get; set; }
        public decimal? conversion_factor { get; set; }
        public string doctype { get; set; }
    }
}