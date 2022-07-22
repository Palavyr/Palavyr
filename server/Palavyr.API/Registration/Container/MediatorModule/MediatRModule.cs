using System;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Handlers.PricingStrategyHandlers;

namespace Palavyr.API.Registration.Container.MediatorModule
{
    public static class MediatorRegistry
    {
        public static void RegisterMediator(IServiceCollection serviceCollection)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName!.Contains("Palavyr.API") || x.FullName.Contains("Palavyr.Core"));
            serviceCollection
                .AddMediatR(assemblies.ToArray())
                .AddTransient<IMediatorServiceTypeConverter, TestConverter>()
                .AddTransient(
                    sp =>
                        MediatorServiceFactory.Wrap(sp.GetService, sp.GetServices<IMediatorServiceTypeConverter>()))
                .AddTransient(typeof(GetPricingStrategyTableRowsHandler<,,>))
                .AddTransient(typeof(GetPricingStrategyTableRowTemplateHandler<,,>))
                .AddTransient(typeof(DeletePricingStrategyTableHandler<,,>))
                .AddTransient(typeof(SavePricingStrategyTableHandler<,,>))
                .AddTransient(typeof(CreatePricingStrategyTableHandler<,,>))
                .BuildServiceProvider();
        }
    }
}