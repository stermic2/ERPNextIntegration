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
                    switch (entity.name)
                    {
                        case "Invoice":
                            responses.Add(await SendConvertedEntityToErpNext<SalesInvoice>("Sales%20Invoice", entity, (await Quickbooks.DataService.GetAsync<Invoice>(entity.Id)).Response!.ToErpNext()));
                            break;
                        case "Item":
                            responses.Add(await SendConvertedEntityToErpNext<ItemRelationship>("Item", entity, (await Quickbooks.DataService.GetAsync<Item>(entity.Id)).Response!.ToErpItem()));
                            break;
                    }
            return responses;
        }

        private async Task<IRestResponse> SendConvertedEntityToErpNext<TIntegrationRelationship>(string endpoint, entity entity, object qboResponse)
        where TIntegrationRelationship : class, IIntegrationRelationship, new()
        {
            var relationshipInIntegrationDatabase = (await Dispatcher.Send(new FindQuery<TIntegrationRelationship>(entity.Id))).Records.FirstOrDefault();
            var erpResponse = await SendRequest(endpoint, entity, qboResponse, relationshipInIntegrationDatabase?.name);
            dynamic erpResponseContent = JObject.Parse(erpResponse.Content);
            if (erpResponse.IsSuccessful) {
                var newRelationship = new TIntegrationRelationship {Id = entity.Id, name = erpResponseContent.data.name};
                await Dispatcher.AddOrUpdate(newRelationship);
            }
            return erpResponse;
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
            entity.errorContent = erpResponse?.Content;
            if (!(erpResponse is {IsSuccessful: true}))
                await Dispatcher.AddOrUpdate(entity);
            else await Dispatcher.Send(new RemoveCommand<entity>(entity.Id));
            return erpResponse;
        }
    }
}