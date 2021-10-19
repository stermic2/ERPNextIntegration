using System.Collections.Generic;
using DynamicCQ.RequestEncapsulation;
using ERPNextIntegration.Dtos.QBO;
using MediatR;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers
{
    public class WebhookEntityProcess<TQboEntity> : IRequest<IEnumerable<IRestResponse>>
    {
        public WebhookEntityProcess(entity entity)
        {
            Entity = entity;
        }
        public entity Entity { get; }
    }
}