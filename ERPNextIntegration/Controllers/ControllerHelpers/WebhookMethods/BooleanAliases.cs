using ERPNextIntegration.IntegrationRelationships;

namespace ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods
{
    public static class BooleanAliases
    {
        public static bool DoesNotExistInTheIntegrationDatabase(this IIntegrationRelationship integrationRelationship)
        {
            return string.IsNullOrEmpty(integrationRelationship.name);
        }
    }
}