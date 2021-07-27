using System;

namespace ERPNextIntegration.Dtos.Webhooks
{
    public class entity
    {
        public string name { get; set; }
        public string id { get; set; }
        public string operation { get; set; }
        public DateTime lastUpdated { get; set; }
    }
}