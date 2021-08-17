using DynamicCQ;
using DynamicCQ.Interfaces;

namespace ERPNextIntegration.IntegrationRelationships
{
    public class ItemRelationship: Entity, IDto, IIntegrationRelationship
    {
        public string name { get; set; }
    }
}