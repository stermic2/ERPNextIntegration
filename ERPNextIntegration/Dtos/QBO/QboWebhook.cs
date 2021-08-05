using System.Collections.Generic;

namespace ERPNextIntegration.Dtos.QBO
{
    public class QboWebhook
    {
        public IEnumerable<eventNotification> eventNotifications { get; set; }
    }
}