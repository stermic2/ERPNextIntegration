using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DynamicCQ.RequestEncapsulation;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using MediatR;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook
{
    public class BillHandler : WebhookProcessBaseHandler, IRequestHandler<WebhookEntityProcess<Bill>, IEnumerable<IRestResponse>>
    {
        public BillHandler(IMediator dispatcher) : base(dispatcher)
        {
        }
        
        public async Task<IEnumerable<IRestResponse>> Handle(WebhookEntityProcess<Bill> request, CancellationToken cancellationToken)
        {
            List<IRestResponse> responsesForThisEntity = new List<IRestResponse>();
            //responsesForThisEntity.Add(await SendConvertedEntityToErpNext<SalesInvoiceRelationship>("Sales%20Invoice", entity, 
            //    (await Quickbooks.DataService.GetAsync<Bill>(entity.Id)).Response!.ToErpInvoice()));
            throw new NotImplementedException();
            return responsesForThisEntity;
        }
    }
}