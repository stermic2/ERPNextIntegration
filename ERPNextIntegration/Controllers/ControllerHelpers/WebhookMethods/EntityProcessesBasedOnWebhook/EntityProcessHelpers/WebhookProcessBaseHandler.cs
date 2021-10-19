using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicCQ.Requests.Queries.FindQuery;
using DynamicCQ.Requests.RequestPipelines;
using ERPNextIntegration.Dtos.ErpNext;
using ERPNextIntegration.Dtos.ErpNext.Wrapper;
using ERPNextIntegration.Dtos.QBO;
using ERPNextIntegration.IntegrationRelationships;
using MediatR;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers
{
    public class WebhookProcessBaseHandler
    {
        public readonly IMediator Dispatcher;

        public WebhookProcessBaseHandler(IMediator dispatcher)
        {
            Dispatcher = dispatcher;
        }

        public async Task<IRestResponse> SendConvertedEntity<TIntegrationRelationship, TErpNextDto>(string endpoint, entity entity, IErpNextDto qboResponse)
        where TIntegrationRelationship : class, IIntegrationRelationship, new()
        where TErpNextDto : IErpNextDto
        {
            IRestResponse erpResponse = null;
            var relationshipInIntegrationDatabase = (await Dispatcher.Send(new FindQuery<TIntegrationRelationship>(entity.Id))).Records.FirstOrDefault();
            if (relationshipInIntegrationDatabase != null)
            {
                if (relationshipInIntegrationDatabase.DoesNotExistInTheIntegrationDatabase())
                {
                    var erpEntity = ErpNext.Client.Get<ErpRequest<List<TErpNextDto>>>(new RestRequest(endpoint + "?filters=[[\"quickbooks_id\", \"=\", \"" + entity.Id + "\"]]"));
                    relationshipInIntegrationDatabase.name = erpEntity.Data.data.FirstOrDefault()?.name;
                }
                relationshipInIntegrationDatabase.name = relationshipInIntegrationDatabase.name?.Replace(" ", "%20");
            }
            switch (entity.operation)
            {
                case "Create":
                    if (!string.IsNullOrEmpty(relationshipInIntegrationDatabase?.name))
                    {
                        //erpResponse = ErpNext.Client.Get<TIntegrationRelationship>(new RestRequest(endpoint + "?filters=[[\"quickbooks_id\", \"=\", \"" + entity.Id + "\"]]"));
                        //if (erpResponse.IsSuccessful)
                    }
                    erpResponse = ErpNext.Client.Post(new RestRequest(endpoint).AddJsonBody(qboResponse));
                    break;
                case "Update":
                    if (string.IsNullOrEmpty(relationshipInIntegrationDatabase?.name))
                        erpResponse = ErpNext.Client.Post(new RestRequest(endpoint).AddJsonBody(qboResponse));
                    else
                    {
                        qboResponse.name = null;
                        erpResponse = ErpNext.Client.Put(new RestRequest(endpoint + "/" + relationshipInIntegrationDatabase?.name)
                        .AddJsonBody(qboResponse));
                    }
                    if (erpResponse.Content.Contains("DoesNotExistError") || erpResponse.Content == "{}")
                        erpResponse = ErpNext.Client.Post(new RestRequest(endpoint).AddJsonBody(qboResponse));
                    break;
                case "Delete":
                    erpResponse = ErpNext.Client.Delete(new RestRequest(endpoint + "/" + relationshipInIntegrationDatabase?.name));
                    break;
            }
            if (erpResponse.Content != "{}" && erpResponse.IsSuccessful) {
                dynamic erpResponseContent = JObject.Parse(erpResponse.Content);
                var newRelationship = new TIntegrationRelationship {Id = entity.Id, name = erpResponseContent.data.name};
                await Dispatcher.AddOrUpdate(newRelationship);
            }
            return erpResponse;
        }
    }
}