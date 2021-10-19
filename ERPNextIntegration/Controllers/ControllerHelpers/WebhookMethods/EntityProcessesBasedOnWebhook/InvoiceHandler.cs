using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DynamicCQ.RequestEncapsulation;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using ERPNextIntegration.Dtos.ErpNext.SalesInvoice;
using ERPNextIntegration.Dtos.QBO.QboExtensions;
using ERPNextIntegration.IntegrationRelationships;
using MediatR;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook
{
    public class InvoiceHandler : WebhookProcessBaseHandler, IRequestHandler<WebhookEntityProcess<Invoice>, IEnumerable<IRestResponse>>
    {
        public InvoiceHandler(IMediator dispatcher) : base(dispatcher)
        {
        }
        
        public async Task<IEnumerable<IRestResponse>> Handle(WebhookEntityProcess<Invoice> request, CancellationToken cancellationToken)
        {
            List<IRestResponse> responsesForThisEntity = new List<IRestResponse>();
            responsesForThisEntity
                .Add(await SendConvertedEntity<SalesInvoiceRelationship, SalesInvoice>("Sales%20Invoice", request.Entity,
                    (await Quickbooks.DataService.GetAsync<Invoice>(request.Entity.Id)).Response!.ToErpInvoice()));
            return responsesForThisEntity;
        }
    }
}