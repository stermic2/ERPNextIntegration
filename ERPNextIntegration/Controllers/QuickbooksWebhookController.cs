using System.Collections.Generic;
using System.Threading.Tasks;
using ERPNextIntegration.Dtos.Webhooks;
using ERPNextIntegration.QBO.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace ERPNextIntegration.Controllers
{
    [Route("api/v1/qbo/webhook")]
    public class QuickbooksWebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QboDto dto)
        {
            var response = new List<object>();
            foreach (var eventNotification in dto.eventNotifications)
            foreach (var entity in eventNotification.dataChangeEvent.entities)
                response.Add(await Quickbooks.Client
                    .For<Item>()
                    .Key(3)
                    .FindEntryAsync());
            return Ok(response);
        }
    }
}