using System.Collections.Generic;
using System.Threading.Tasks;
using ERPNextIntegration.Dtos.QBO;
using Microsoft.AspNetCore.Mvc;
using QuickBooksSharp;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers
{
    [Route("api/v1/qbo/webhook")]
    public class QuickbooksWebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QboWebhook webhook)
        {
            var response = new List<QuickBooksSharp.Entities.Invoice>();
            await Quickbooks.RefreshTokens(); 
            try
            {
                await ProcessWebhook(webhook);
            }
            catch (QuickBooksException e)
            {
                //if (!e.Message.Contains("statusCode=401")) throw;
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
                            Invoice response = (await Quickbooks.DataService.GetAsync<Invoice>(entity.id)).Response!;
                            var requestBody = response?.ToErpNext();
                            await ProcessEntity("Sales%20Invoice", entity, requestBody);
                            break;
                    }
        }

        private async Task ProcessEntity(string endpoint, entity entity, object requestBody)
        {
            IRestResponse erpResponse;
            switch (entity.operation)
            {
                case "Create":
                    erpResponse = ErpNext.Client.Post(new RestRequest(endpoint).AddJsonBody(requestBody));
                    break;
                case "Update":
                    ErpNext.Client.Put(new RestRequest(endpoint + "/" + entity.id).AddJsonBody(requestBody));
                    break;
            }
        }
    }
}