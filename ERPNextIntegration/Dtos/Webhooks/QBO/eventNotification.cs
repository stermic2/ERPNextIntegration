using ERPNextIntegration.Dtos.Webhooks.QBO;

namespace ERPNextIntegration.Dtos.Webhooks
{
    public class eventNotification
    {
        public string realmId { get; set; }
        public dataChangeEvent dataChangeEvent { get; set; }
    }
}