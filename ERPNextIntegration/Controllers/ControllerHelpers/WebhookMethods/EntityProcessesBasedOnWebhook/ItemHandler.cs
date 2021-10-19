using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DynamicCQ.RequestEncapsulation;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using ERPNextIntegration.Dtos.ErpNext.Item;
using ERPNextIntegration.Dtos.QBO.QboExtensions;
using ERPNextIntegration.IntegrationRelationships;
using MediatR;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook
{
    public class ItemHandler : WebhookProcessBaseHandler, IRequestHandler<WebhookEntityProcess<Item>, IEnumerable<IRestResponse>>
    {
        public ItemHandler(IMediator dispatcher) : base(dispatcher)
        {
        }

        public async Task<IEnumerable<IRestResponse>> Handle(WebhookEntityProcess<Item> request, CancellationToken cancellationToken)
        {
            List<IRestResponse> responsesForThisEntity = new List<IRestResponse>();
            responsesForThisEntity.Add(await SendConvertedEntity<ItemRelationship, ErpItem>("Item",
                request.Entity,
                (await Quickbooks.DataService.GetAsync<Item>(request.Entity.Id)).Response!.ToErpItem()));
            return responsesForThisEntity;
        }
    }
}