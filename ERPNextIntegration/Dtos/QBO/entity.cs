using System;
using DynamicCQ;
using DynamicCQ.Interfaces;

namespace ERPNextIntegration.Dtos.QBO
{
    public class entity: Entity, IDto
    {
        public string name { get; set; }
        public string operation { get; set; }
        public DateTime lastUpdated { get; set; }
    }
}