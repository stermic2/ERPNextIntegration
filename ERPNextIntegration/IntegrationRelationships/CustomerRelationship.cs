using DynamicCQ;
using DynamicCQ.Interfaces;

namespace ERPNextIntegration.IntegrationRelationships
{
    public class CustomerRelationship: Entity, IDto, IIntegrationRelationship
    {
        public string name { get; set; }
    }
}