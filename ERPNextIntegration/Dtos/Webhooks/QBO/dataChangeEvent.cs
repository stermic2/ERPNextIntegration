using System.Collections;
using System.Collections.Generic;

namespace ERPNextIntegration.Dtos.Webhooks.QBO
{
    public class dataChangeEvent
    {
        public IEnumerable<entity> entities { get; set; }
    }
}