using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DynamicCQ.Requests.RequestPipelines;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using ERPNextIntegration.Dtos.QBO;
using MediatR;
using Microsoft.AspNetCore.Components;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers
{
    public class WebhookProcessor
    {
        private readonly IMediator _dispatcher;
        private readonly List<IRestResponse> _responsesForThisWebhook;
        private readonly EntityProcessor _entityProcessor;

        public WebhookProcessor(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
            _entityProcessor = new EntityProcessor(dispatcher);
            _responsesForThisWebhook = new List<IRestResponse>();
        }

        public async Task<ICollection<IRestResponse>> ProcessWebhook(QboWebhook webhook)
        {
            foreach (var notification in webhook.eventNotifications) 
                await ProcessEntities(notification);
            return _responsesForThisWebhook;
        }

        private async Task ProcessEntities(eventNotification notification)
        {
            foreach (var entity in notification.dataChangeEvent.entities)
                _responsesForThisWebhook.AddRange(await _entityProcessor.ProcessEntity(entity));
        }
    }
}