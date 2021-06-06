using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers;
using Stripe;

namespace Palavyr.API.Registration.Container
{
    public class StripeModule : Module
    {
        private readonly IConfiguration configuration;

        public StripeModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private static readonly int stripeRetriesCount = 3;

        protected override void Load(ContainerBuilder builder)
        {
            var currentEnv = configuration.GetCurrentEnvironment();
            var stripeKey = configuration.GetStripeKey();
            if (currentEnv != DetermineCurrentEnvironment.Production)
            {
                if (stripeKey.ToLowerInvariant().StartsWith("sk_live_"))
                {
                    throw new Exception("CRITICAL - attempting to use a production stripe API key in test environment - CRITICAL");
                } 
            }
            
            StripeConfiguration.ApiKey = stripeKey;
            StripeConfiguration.MaxNetworkRetries = stripeRetriesCount;

            builder.RegisterType<StripeWebhookAuthService>().AsSelf();
            builder.RegisterType<StripeEventWebhookService>().AsSelf();
            builder.RegisterType<StripeCustomerService>().AsSelf();
            builder.RegisterType<StripeSubscriptionService>().AsSelf();
            builder.RegisterType<StripeProductService>().AsSelf();
            builder.RegisterType<StripeCheckoutService>().AsSelf();

            builder.RegisterType<ProcessStripeSubscriptionUpdatedHandler>().AsSelf();
            builder.RegisterType<ProcessStripeSubscriptionDeletedHandler>().AsSelf();
            builder.RegisterType<ProcessStripeCheckoutSessionCompletedHandler>().AsSelf();
            builder.RegisterType<ProcessStripeInvoicePaidHandler>().AsSelf();
            builder.RegisterType<ProcessStripeInvoicePaymentFailedHandler>().AsSelf();
            builder.RegisterType<ProcessStripeSubscriptionCreatedHandler>().AsSelf();
            builder.RegisterType<ProcessStripePaymentMethodUpdatedHandler>().AsSelf();
            builder.RegisterType<ProcessStripePlanUpdatedHandler>().AsSelf();
            builder.RegisterType<ProcessStripeInvoiceCreatedHandler>().AsSelf();
            builder.RegisterType<ProcessStripePriceUpdatedHandler>().AsSelf();

            builder.RegisterType<StripeCustomerManagementPortalService>().As<IStripeCustomerManagementPortalService>();

            builder.Register<IProductRegistry>(
                    context =>
                    {
                        var determineCurrentEnvironment = new DetermineCurrentEnvironment(configuration);
                        if (determineCurrentEnvironment.IsProduction())
                        {
                            return new ProductionProductRegistry();
                        }
                        else
                        {
                            return new StagingProductRegistry();
                        }
                    })
                .As<IProductRegistry>()
                .InstancePerLifetimeScope();
        }
    }
}