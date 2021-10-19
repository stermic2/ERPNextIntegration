using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicCQ.RequestEncapsulation;
using DynamicCQ.Requests.Queries.FindQuery;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using ERPNextIntegration.Dtos.ErpNext.PaymentEntry;
using ERPNextIntegration.Dtos.QBO.QboExtensions;
using ERPNextIntegration.IntegrationRelationships;
using MediatR;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook
{
    public class PaymentHandler : WebhookProcessBaseHandler, IRequestHandler<WebhookEntityProcess<Payment>, IEnumerable<IRestResponse>>
    {
        public PaymentHandler(IMediator dispatcher) : base(dispatcher)
        {
        }
        
        public async Task<IEnumerable<IRestResponse>> Handle(WebhookEntityProcess<Payment> request, CancellationToken cancellationToken)
        {
            List<IRestResponse> responsesForThisEntity = new List<IRestResponse>();
            var relatedInvoices = (await Quickbooks.DataService.GetAsync<Payment>(request.Entity.Id)).Response!;
            IEnumerable<Task<SalesInvoiceRelationship>> relatedSalesInvoicesQueries =
                relatedInvoices?.Line?.FirstOrDefault()?.LinkedTxn?.Select(async x =>
                    (await Dispatcher.Send(new FindQuery<SalesInvoiceRelationship>(x.TxnId), cancellationToken)).Records.FirstOrDefault());
            IEnumerable<SalesInvoiceRelationship> relatedSalesInvoices = await Task.WhenAll(relatedSalesInvoicesQueries!);
            responsesForThisEntity.Add(
                await SendConvertedEntity<PaymentEntryRelationship, ErpPaymentEntry>("Payment%20Entry", request.Entity, relatedInvoices.ToErpPaymentEntry(relatedSalesInvoices)));
            return responsesForThisEntity;
        }
    }
}