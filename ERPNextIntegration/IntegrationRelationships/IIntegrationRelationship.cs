using DynamicCQ.Interfaces;

namespace ERPNextIntegration.IntegrationRelationships
{
    public interface IIntegrationRelationship : IDto
    {
        public string Id { get; set; }
        public string name { get; set; }
    }
}