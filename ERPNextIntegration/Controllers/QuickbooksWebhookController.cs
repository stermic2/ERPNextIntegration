using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using DynamicCQ.Controllers;
using DynamicCQ.Requests.RequestPipelines;
using ERPNextIntegration.Controllers.ControllerHelpers;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using ERPNextIntegration.Dtos.QBO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuickBooksSharp;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers
{
    [Route("api/v1/qbo/webhook")]
    public class QuickbooksWebhookController : DynamicCqControllerBase
    {
        private readonly WebhookProcessor _webhookProcessor;

        public QuickbooksWebhookController(IMapper mapper, IMediator dispatcher) : base(mapper, dispatcher)
        {
            _webhookProcessor = new WebhookProcessor(dispatcher);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QboWebhook webhook)
        {
            ICollection<IRestResponse> responses;
            await Quickbooks.RefreshTokens();
            try
            {
                responses = await _webhookProcessor.ProcessWebhook(webhook);
            }
            catch (QuickBooksException e)
            {
                if (!e.Message.Contains("statusCode=401"))
                    return NotFound(e.Message);
                await Quickbooks.ForceRefresh();
                try
                {
                    responses = await _webhookProcessor.ProcessWebhook(webhook);
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
    }
}