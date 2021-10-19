using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicCQ.RequestEncapsulation;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using ERPNextIntegration.Dtos.ErpNext.Customer;
using ERPNextIntegration.Dtos.QBO.QboExtensions;
using ERPNextIntegration.IntegrationRelationships;
using MediatR;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook
{
    public class CustomerHandler : WebhookProcessBaseHandler, IRequestHandler<WebhookEntityProcess<Customer>, IEnumerable<IRestResponse>>
    {
        public CustomerHandler(IMediator dispatcher) : base(dispatcher)
        {
        }
        
        public async Task<IEnumerable<IRestResponse>> Handle(WebhookEntityProcess<Customer> request, CancellationToken cancellationToken)
        {
            List<IRestResponse> responsesForThisEntity = new List<IRestResponse>();
            var customer = (await Quickbooks.DataService.GetAsync<Customer>(request.Entity.Id)).Response!;
            responsesForThisEntity.Add(await SendConvertedEntity<CustomerRelationship, ErpCustomer>("Customer", request.Entity, customer.ToErpCustomer()));
            if(customer?.BillAddr != null && responsesForThisEntity.Last().IsSuccessful)
                responsesForThisEntity.Add(await SendConvertedEntity<CustomerAddressRelationship, ErpAddress>("Address", request.Entity, customer.ToErpBillingAddress()));
            if(customer?.ShipAddr != null && responsesForThisEntity.Last().IsSuccessful)
                responsesForThisEntity.Add(await SendConvertedEntity<CustomerAddressRelationship, ErpAddress>("Address", request.Entity, customer.ToErpShippingAddress()));
            return responsesForThisEntity;
        }
    }
}