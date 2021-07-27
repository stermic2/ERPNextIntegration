using System.Collections.Generic;

namespace ERPNextIntegration.Dtos.Webhooks
{
    public class QboDto
    {
        public IEnumerable<eventNotification> eventNotifications { get; set; }
    }
}