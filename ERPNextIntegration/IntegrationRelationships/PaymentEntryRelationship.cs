using DynamicCQ;
using DynamicCQ.Interfaces;

namespace ERPNextIntegration.IntegrationRelationships
{
    public class PaymentEntryRelationship: Entity, IDto, IIntegrationRelationship
    {
        public string name { get; set; }
    }
}