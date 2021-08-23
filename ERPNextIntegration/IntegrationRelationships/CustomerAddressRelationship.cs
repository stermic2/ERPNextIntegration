using DynamicCQ;
using DynamicCQ.Interfaces;

namespace ERPNextIntegration.IntegrationRelationships
{
    public class CustomerAddressRelationship: Entity, IDto, IIntegrationRelationship
    {
        public string name { get; set; }
    }
}