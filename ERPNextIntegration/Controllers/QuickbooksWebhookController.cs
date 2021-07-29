using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ERPNextIntegration.Dtos.Webhooks;
using Microsoft.AspNetCore.Mvc;
using QuickBooksSharp;
using QuickBooksSharp.Entities;

namespace ERPNextIntegration.Controllers
{
    [Route("api/v1/qbo/webhook")]
    public class QuickbooksWebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QboDto dto)
        {
            var response = new List<IntuitResponse<IntuitEntity>>();
            await Quickbooks.RefreshTokens();
            Assembly[] assembly = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var notification in dto.eventNotifications) 
                foreach (var entity in notification.dataChangeEvent.entities)
                    response.Add(await Quickbooks.DataService.GetAsync(entity.id, Type.GetType("QuickBooksSharp.Entities." + entity.name)!));
            return Ok(response);
        }
    }
}