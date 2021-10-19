using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DynamicCQ.Requests.RequestPipelines;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using ERPNextIntegration.Dtos.QBO;
using MediatR;
using QuickBooksSharp.Entities;
using RestSharp;

namespace ERPNextIntegration.Controllers.ControllerHelpers
{
    public class EntityProcessor
    {
        private readonly IMediator _dispatcher;
        private readonly List<IRestResponse> _responsesForThisEntity = new List<IRestResponse>();
        private static entity _entity;

        public EntityProcessor(IMediator dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task<List<IRestResponse>> ProcessEntity(entity entity)
        {
            _entity = entity;
            List<IRestResponse> responsesForThisEntity = new List<IRestResponse>();
            await SendToErpNext();
            return responsesForThisEntity;
        }

        private async Task SendToErpNext()
        {
            var commandInstance = CreateGenericWebhookCommand();
            if (commandInstance != null) _responsesForThisEntity
                .AddRange((IEnumerable<IRestResponse>) await _dispatcher.Send(commandInstance) 
                          ?? throw new InvalidOperationException("Expected " + nameof(IEnumerable<IRestResponse>)));
            foreach (var response in _responsesForThisEntity)
                await SaveErrorResponse(response);
        }

        private static object CreateGenericWebhookCommand()
        {
            Type genericWebhookProcessType = typeof(WebhookEntityProcess<>)
                .MakeGenericType(Assembly.GetAssembly(typeof(IntuitEntity))?
                    .GetType($"QuickBooksSharp.Entities.{_entity.name.Replace(" ", "")}") 
                                 ?? throw new InvalidOperationException());
            return Activator.CreateInstance(genericWebhookProcessType, _entity);
        }

        private async Task SaveErrorResponse(IRestResponse response)
        {
            _entity.errorContent += response.Content;
            if (!(response is { IsSuccessful: true }))
                await _dispatcher.AddOrUpdate(_entity);
        }
    }
}