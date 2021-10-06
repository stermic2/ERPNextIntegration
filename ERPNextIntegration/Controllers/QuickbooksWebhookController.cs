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
using DynamicCQ.Requests.Queries.HandlerHelpers.Exceptions;
using DynamicCQ.Requests.RequestPipelines;
using ERPNextIntegration.Dtos.ErpNext;
using ERPNextIntegration.Dtos.ErpNext.Customer;
using ERPNextIntegration.Dtos.ErpNext.Item;
using ERPNextIntegration.Dtos.ErpNext.PaymentEntry;
using ERPNextIntegration.Dtos.ErpNext.SalesInvoice;
using ERPNextIntegration.Dtos.ErpNext.Supplier;
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
                try
                {
                    responses = await ProcessWebhook(webhook);
                }
                catch
                {
                    return Unauthorized(e.Message);
                }
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
                            responsesForThisEntity.Add(await SendConvertedEntityToErpNext<SalesInvoiceRelationship, SalesInvoice>("Sales%20Invoice", entity, 
                                (await Quickbooks.DataService.GetAsync<Invoice>(entity.Id)).Response!.ToErpInvoice()));
                            break;
                        case "Sales Receipt":
                            responsesForThisEntity.Add(await SendConvertedEntityToErpNext<SalesInvoiceRelationship, SalesInvoice>("Sales%20Invoice", entity, 
                                (await Quickbooks.DataService.GetAsync<SalesReceipt>(entity.Id)).Response!.ToErpInvoice()));
                            break;
                        case "Item":
                            responsesForThisEntity.Add(await SendConvertedEntityToErpNext<ItemRelationship, ErpItem>("Item", entity, 
                                (await Quickbooks.DataService.GetAsync<Item>(entity.Id)).Response!.ToErpItem()));
                            break;
                        case "Customer":
                            var customer = (await Quickbooks.DataService.GetAsync<Customer>(entity.Id)).Response!;
                            responsesForThisEntity.Add(await SendConvertedEntityToErpNext<CustomerRelationship, ErpCustomer>("Customer", entity, customer.ToErpCustomer()));
                            if(customer?.BillAddr != null && responsesForThisEntity.Last().IsSuccessful)
                                responsesForThisEntity.Add(await SendConvertedEntityToErpNext<CustomerAddressRelationship, ErpAddress>("Address", entity, customer.ToErpBillingAddress()));
                            if(customer?.ShipAddr != null && responsesForThisEntity.Last().IsSuccessful)
                                responsesForThisEntity.Add(await SendConvertedEntityToErpNext<CustomerAddressRelationship, ErpAddress>("Address", entity, customer.ToErpShippingAddress()));
                            break;
                        case "Payment":
                            var relatedInvoices = (await Quickbooks.DataService.GetAsync<Payment>(entity.Id)).Response!;
                            IEnumerable<Task<SalesInvoiceRelationship>> relatedSalesInvoicesQueries =
                                relatedInvoices?.Line?.FirstOrDefault()?.LinkedTxn?.Select(async x =>
                                    (await Dispatcher.Send(new FindQuery<SalesInvoiceRelationship>(x.TxnId))).Records.FirstOrDefault());
                            IEnumerable<SalesInvoiceRelationship> relatedSalesInvoices = await Task.WhenAll(relatedSalesInvoicesQueries!);
                            responsesForThisEntity.Add(
                                await SendConvertedEntityToErpNext<PaymentEntryRelationship, ErpPaymentEntry>("Payment%20Entry", entity, relatedInvoices.ToErpPaymentEntry(relatedSalesInvoices)));
                            break;
                        case "Purchase Order":
                            //responsesForThisEntity.Add(await SendConvertedEntityToErpNext<SalesInvoiceRelationship>("Sales%20Invoice", entity, 
                            //    (await Quickbooks.DataService.GetAsync<PurchaseOrder>(entity.Id)).Response!.ToErpInvoice()));
                            break;
                        case "Bill":
                            //responsesForThisEntity.Add(await SendConvertedEntityToErpNext<SalesInvoiceRelationship>("Sales%20Invoice", entity, 
                            //    (await Quickbooks.DataService.GetAsync<Bill>(entity.Id)).Response!.ToErpInvoice()));
                            break;
                        case "Vendor":
                            responsesForThisEntity.Add(await SendConvertedEntityToErpNext<SupplierRelationship, ErpSupplier>("Supplier", entity, 
                                (await Quickbooks.DataService.GetAsync<Vendor>(entity.Id)).Response!.ToErpSupplier()));
                            break;
                        default:
                            throw new NotImplementedException();
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

        private async Task<IRestResponse> SendConvertedEntityToErpNext<TIntegrationRelationship, TErpNextDto>(string endpoint, entity entity, IErpNextDto qboResponse)
        where TIntegrationRelationship : class, IIntegrationRelationship, new()
        where TErpNextDto : IErpNextDto
        {
            IRestResponse erpResponse = null;
            var relationshipInIntegrationDatabase = (await Dispatcher.Send(new FindQuery<TIntegrationRelationship>(entity.Id))).Records.FirstOrDefault();
            if (relationshipInIntegrationDatabase != null)
            {
                if (string.IsNullOrEmpty(relationshipInIntegrationDatabase.name))
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
                        //if (erpResponse.IsSuccessful) Latest edition
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