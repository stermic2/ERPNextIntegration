using Autofac;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using DynamicCQ.AutofacExtensions;
using DynamicCQ.RequestEncapsulation;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook;
using ERPNextIntegration.Controllers.ControllerHelpers.WebhookMethods.EntityProcessesBasedOnWebhook.EntityProcessHelpers;
using ERPNextIntegration.Dtos.QBO;
using ERPNextIntegration.IntegrationRelationships;
using MediatR;
using QuickBooksSharp.Entities;

namespace ERPNextIntegration
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddMaps(typeof(AutofacModule).Assembly);
                builder.GenericallyRegisterAnEntityToDto<IntegrationDbContext, SalesInvoiceRelationship, SalesInvoiceRelationship>(cfg);
                builder.GenericallyRegisterAnEntityToDto<IntegrationDbContext, ItemRelationship, ItemRelationship>(cfg);
                builder.GenericallyRegisterAnEntityToDto<IntegrationDbContext, CustomerRelationship, CustomerRelationship>(cfg);
                builder.GenericallyRegisterAnEntityToDto<IntegrationDbContext, CustomerAddressRelationship, CustomerAddressRelationship>(cfg);
                builder.GenericallyRegisterAnEntityToDto<IntegrationDbContext, PaymentEntryRelationship, PaymentEntryRelationship>(cfg);
                builder.GenericallyRegisterAnEntityToDto<IntegrationDbContext, SupplierRelationship, SupplierRelationship>(cfg);
                builder.GenericallyRegisterAnEntityToDto<IntegrationDbContext, entity, entity>(cfg);
            });
            builder.Register(c =>
                {
                    var mapper = mapperCfg.CreateMapper();
                    return mapper;
                }).AsImplementedInterfaces().InstancePerLifetimeScope()
                .Named<IMapper>(nameof(ERPNextIntegration));
            
            builder.RegisterAssemblyTypes(typeof(AutofacModule).Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>)).AsImplementedInterfaces().InstancePerDependency();
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}