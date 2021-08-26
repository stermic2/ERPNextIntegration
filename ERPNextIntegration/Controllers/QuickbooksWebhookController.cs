using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DynamicCQ.Controllers;
using DynamicCQ.Requests.Commands.AddCommand;
using DynamicCQ.Requests.Commands.RemoveCommand;
using DynamicCQ.Requests.Commands.UpdateCommand;
using DynamicCQ.Requests.Queries.FindQuery;
using DynamicCQ.Requests.RequestPipelines;
using ERPNextIntegration.Dtos.ErpNext.Wrapper;
using ERPNextIntegration.Dtos.QBO;
using ERPNextIntegration.Dtos.QBO.QboExtensions;
using ERPNextIntegration.IntegrationRelationships;
using Flurl.Util;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickBooksSharp;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers
{
    [Route("api/v1/qbo/webhook")]
    public class QuickbooksWebhookController : DynamicCqControllerBase
    {
        public QuickbooksWebhookController(IMapper mapper, IMediator dispatcher) : base(mapper, dispatcher)
        {
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QboWebhook webhook)
        {
            ICollection<IRestResponse> responses;
            await Quickbooks.RefreshTokens(); 
            try
            {
                responses = await ProcessWebhook(webhook);
            }
            catch (QuickBooksException e)
            {
                if (!e.Message.Contains("statusCode=401"))
                    return NotFound(e.Message);
                await Quickbooks.ForceRefresh();
                responses = await ProcessWebhook(webhook);
            }
            if (responses.Any(x => !x.IsSuccessful))
                return BadRequest(responses.Select(x => x.Content));
            return Ok(responses.Select(x => x.Content));
        }

        private async Task<ICollection<IRestResponse>> ProcessWebhook(QboWebhook webhook)
        {
            List<IRestResponse> responses = new List<IRestResponse>();
            foreach (var notification in webhook.eventNotifications)
                foreach (var entity in notification.dataChangeEvent.entities)
                {
                    List<IRestResponse> responsesForThisEntity = new List<IRestResponse>();
                    switch (entity.name)
                    {
                        case "Invoice":
                            responsesForThisEntity.Add(await SendConvertedEntityToErpNext<SalesInvoiceRelationship>("Sales%20Invoice", entity, 
                                (await Quickbooks.DataService.GetAsync<Invoice>(entity.Id)).Response!.ToErpInvoice()));
                            break;
                        case "Item":
                            responsesForThisEntity.Add(await SendConvertedEntityToErpNext<ItemRelationship>("Item", entity, 
                                (await Quickbooks.DataService.GetAsync<Item>(entity.Id)).Response!.ToErpItem()));
                            break;
                        case "Customer":
                            var customer = (await Quickbooks.DataService.GetAsync<Customer>(entity.Id)).Response!;
                            responsesForThisEntity.Add(await SendConvertedEntityToErpNext<CustomerRelationship>("Customer", entity, customer.ToErpCustomer()));
                            if(customer?.BillAddr != null && responsesForThisEntity.Any(x => x.IsSuccessful))
                                responsesForThisEntity.Add(await SendConvertedEntityToErpNext<CustomerAddressRelationship>("Address", entity, customer.ToErpBillingAddress()));
                            if(customer?.ShipAddr != null && responsesForThisEntity.Any(x => x.IsSuccessful))
                                responsesForThisEntity.Add(await SendConvertedEntityToErpNext<CustomerAddressRelationship>("Address", entity, customer.ToErpShippingAddress()));
                            break;
                        case "Payment":
                            var relatedInvoices = (await Quickbooks.DataService.GetAsync<Payment>(entity.Id)).Response!;
                            IEnumerable<Task<SalesInvoiceRelationship>> relatedSalesInvoicesQueries =
                                relatedInvoices?.Line?.FirstOrDefault()?.LinkedTxn?.Select(async x =>
                                    (await Dispatcher.Send(new FindQuery<SalesInvoiceRelationship>(x.TxnId))).Records.FirstOrDefault());
                            IEnumerable<SalesInvoiceRelationship> relatedSalesInvoices = await Task.WhenAll(relatedSalesInvoicesQueries);
                            responsesForThisEntity.Add(
                                await SendConvertedEntityToErpNext<SalesInvoiceRelationship>("Payment%20Entry", entity, relatedInvoices.ToErpPaymentEntry(relatedSalesInvoices)));
                            break;
                    }
                    foreach (var response in responsesForThisEntity)
                    {
                        entity.errorContent += response.Content;
                        if (!(response is {IsSuccessful: true}))
                            await Dispatcher.AddOrUpdate(entity);
                        //else await Dispatcher.Send(new RemoveCommand<entity>(entity.Id));
                    }
                    responses.AddRange(responsesForThisEntity);
                }
            return responses;
        }

        private async Task<IRestResponse> SendConvertedEntityToErpNext<TIntegrationRelationship>(string endpoint, entity entity, object qboResponse)
        where TIntegrationRelationship : class, IIntegrationRelationship, new()
        {
            var relationshipInIntegrationDatabase = (await Dispatcher.Send(new FindQuery<TIntegrationRelationship>(entity.Id))).Records.FirstOrDefault();
            var erpResponse = SendRequest(endpoint, entity, qboResponse, relationshipInIntegrationDatabase?.name);
            dynamic erpResponseContent = JObject.Parse(erpResponse.Content);
            if (erpResponse.IsSuccessful) {
                var newRelationship = new TIntegrationRelationship {Id = entity.Id, name = erpResponseContent.data.name};
                await Dispatcher.AddOrUpdate(newRelationship);
            }
            return erpResponse;
        }

        private IRestResponse SendRequest(string endpoint, entity entity, object requestBody, string name)
        {
            name = name?.Replace(" ", "%20");
            IRestResponse erpResponse = null;
            switch (entity.operation)
            {
                case "Create":
                    erpResponse = ErpNext.Client.Post(new RestRequest(endpoint).AddJsonBody(requestBody));
                    break;
                case "Update":
                    erpResponse = ErpNext.Client.Put(new RestRequest(endpoint + "/" + name).AddJsonBody(requestBody));
                    if (erpResponse.Content.Contains("DoesNotExistError") || erpResponse.Content == "{}")
                        erpResponse = ErpNext.Client.Post(new RestRequest(endpoint).AddJsonBody(requestBody));
                    break;
                case "Delete":
                    erpResponse = ErpNext.Client.Delete(new RestRequest(endpoint + "/" + name));
                    break;
            }
            return erpResponse;
        }
    }
}