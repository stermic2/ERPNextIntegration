using DynamicCQ;
using DynamicCQ.Interfaces;

namespace ERPNextIntegration.IntegrationRelationships
{
    public class SalesInvoice: Entity, IDto, IIntegrationRelationship
    {
        public string name { get; set; }
    }
}