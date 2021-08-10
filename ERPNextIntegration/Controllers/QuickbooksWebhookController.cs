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
                            ProcessEntity("Sales%20Invoice", entity, (await Quickbooks.DataService.GetAsync<Invoice>(entity.id)).Response!.ToErpNext());
                            break;
                    }
        }

        private void ProcessEntity(string endpoint, entity entity, object requestBody)
        {
            IRestResponse erpResponse = null;
            switch (entity.operation)
            {
                case "Create":
                    erpResponse = ErpNext.Client.Post(new RestRequest(endpoint).AddJsonBody(requestBody));
                    break;
                case "Update":
                    erpResponse = ErpNext.Client.Put(new RestRequest(endpoint + "/" + entity.id).AddJsonBody(requestBody));
                    break;
            }
            //if (!(erpResponse is {IsSuccessful: true}))
                
        }
    }
}