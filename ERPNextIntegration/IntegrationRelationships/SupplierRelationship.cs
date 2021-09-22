using DynamicCQ;
using DynamicCQ.Interfaces;

namespace ERPNextIntegration.IntegrationRelationships
{
    public class SupplierRelationship: Entity, IDto, IIntegrationRelationship
    {
        public string name { get; set; }
    }
}