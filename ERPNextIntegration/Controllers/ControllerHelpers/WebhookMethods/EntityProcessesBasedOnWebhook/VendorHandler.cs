using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DynamicCQ.RequestEncapsulation;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using ERPNextIntegration.Dtos.ErpNext.Supplier;
using ERPNextIntegration.Dtos.QBO;
using ERPNextIntegration.Dtos.QBO.QboExtensions;
using ERPNextIntegration.IntegrationRelationships;
using MediatR;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook
{
    public class VendorHandler : WebhookProcessBaseHandler, IRequestHandler<WebhookEntityProcess<Vendor>, IEnumerable<IRestResponse>>
    {
        public VendorHandler(IMediator dispatcher) : base(dispatcher)
        {
        }
        
        public async Task<IEnumerable<IRestResponse>> Handle(WebhookEntityProcess<Vendor> request, CancellationToken cancellationToken)
        {
            List<IRestResponse> responsesForThisEntity = new List<IRestResponse>();
            responsesForThisEntity.Add(await SendConvertedEntity<SupplierRelationship, ErpSupplier>("Supplier", request.Entity, 
                (await Quickbooks.DataService.GetAsync<Vendor>(request.Entity.Id)).Response!.ToErpSupplier()));
            return responsesForThisEntity;
        }
    }
}