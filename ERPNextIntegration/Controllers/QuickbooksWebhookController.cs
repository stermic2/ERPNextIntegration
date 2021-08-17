using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DynamicCQ.Controllers;
using DynamicCQ.Requests.Commands.AddCommand;
using DynamicCQ.Requests.Commands.UpdateCommand;
using DynamicCQ.Requests.Queries.FindQuery;
using DynamicCQ.Requests.RequestPipelines;
using ERPNextIntegration.Dtos.ErpNext.Wrapper;
using ERPNextIntegration.Dtos.QBO;
using ERPNextIntegration.Dtos.QBO.QboExtensions;
using ERPNextIntegration.IntegrationRelationships;
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
            var response = new List<Invoice>();
            await Quickbooks.RefreshTokens(); 
            try
            {
                await ProcessWebhook(webhook);
            }
            catch (QuickBooksException e)
            {
                if (!e.Message.Contains("statusCode=401"))
                    return NotFound(e.Message);
                await Quickbooks.ForceRefresh();
                await ProcessWebhook(webhook);
            }
            return Ok(response);
        }

        private async Task ProcessWebhook(QboWebhook webhook)
        {
            foreach (var notification in webhook.eventNotifications)
                foreach (var entity in notification.dataChangeEvent.entities)
                    switch (entity.name)
                    {
                        case "Invoice":
                            await SendConvertedEntityToErpNext<SalesInvoice>("Sales%20Invoice", entity, (await Quickbooks.DataService.GetAsync<Invoice>(entity.Id)).Response!.ToErpNext());
                            break;
                        case "Item":
                            await SendConvertedEntityToErpNext<ItemRelationship>("Item", entity, (await Quickbooks.DataService.GetAsync<Item>(entity.Id)).Response!.ToErpItem());
                            break;
                    }
        }

        private async Task SendConvertedEntityToErpNext<TIntegrationRelationship>(string endpoint, entity entity, object qboResponse)
        where TIntegrationRelationship : class, IIntegrationRelationship, new()
        {
            var relationshipInIntegrationDatabase = (await Dispatcher.Send(new FindQuery<TIntegrationRelationship>(entity.Id))).Records.FirstOrDefault();
            dynamic erpResponse = JObject.Parse((await SendRequest(endpoint, entity, qboResponse, relationshipInIntegrationDatabase?.name)).Content);
            var newRelationship = new TIntegrationRelationship {Id = entity.Id, name = erpResponse.data.name};
            await Dispatcher.AddOrUpdate(newRelationship);
        }

        private async Task<IRestResponse> SendRequest(string endpoint, entity entity, object requestBody, string name)
        {
            IRestResponse erpResponse = null;
            switch (entity.operation)
            {
                case "Create":
                    erpResponse = ErpNext.Client.Post(new RestRequest(endpoint).AddJsonBody(requestBody));
                    break;
                case "Update":
                    erpResponse = ErpNext.Client.Put(new RestRequest(endpoint + "/" + name).AddJsonBody(requestBody));
                    break;
                case "Delete":
                    erpResponse = ErpNext.Client.Delete(new RestRequest(endpoint + "/" + name));
                    break;
            }
            if (!(erpResponse is {IsSuccessful: true}))
                await Dispatcher.AddOrUpdate(entity);
            return erpResponse;
        }
    }
}