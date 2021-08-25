using DynamicCQ;
using DynamicCQ.Interfaces;

namespace ERPNextIntegration.IntegrationRelationships
{
    public class SalesInvoiceRelationship: Entity, IDto, IIntegrationRelationship
    {
        public string name { get; set; }
    }
}