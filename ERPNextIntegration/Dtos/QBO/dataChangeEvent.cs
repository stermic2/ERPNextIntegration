using System.Collections.Generic;

namespace ERPNextIntegration.Dtos.QBO
{
    public class dataChangeEvent
    {
        public IEnumerable<entity> entities { get; set; }
    }
}