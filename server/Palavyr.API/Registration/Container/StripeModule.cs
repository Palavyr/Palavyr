using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Services.StripeServices.Products;
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
            if (currentEnv != DetermineCurrentEnvironment.Production && stripeKey.ToLowerInvariant().StartsWith("sk_live_"))
            {
                throw new Exception("CRITICAL ERROR - attempting to use a production stripe API key in test environment - CRITICAL");
            }

            StripeConfiguration.ApiKey = stripeKey;
            StripeConfiguration.MaxNetworkRetries = stripeRetriesCount;

            builder.RegisterType<StripeEventWebhookRoutingService>().As<IStripeEventWebhookRoutingService>();
            builder.RegisterType<StripeWebhookAuthService>().As<IStripeWebhookAuthService>();
            builder.RegisterType<StripeSubscriptionService>().As<IStripeSubscriptionService>();
            builder.RegisterType<BillingPortalSession>().As<IBillingPortalSession>().InstancePerLifetimeScope();
            builder.RegisterType<StripeCheckoutServiceSession>().As<IStripeCheckoutServiceSession>().InstancePerLifetimeScope();
            builder.RegisterType<StripeCustomerService>().As<IStripeCustomerService>().InstancePerDependency();
            builder.RegisterType<CustomerSessionService>().As<ICustomerSessionService>().InstancePerLifetimeScope();
            builder.RegisterType<StripeProductService>().As<IStripeProductService>().InstancePerDependency();
            builder.RegisterType<StripeCustomerManagementPortalService>().As<IStripeCustomerManagementPortalService>();
            builder.RegisterType<StripeSubscriptionRetriever>().As<IStripeSubscriptionRetriever>();

            builder.Register(
                    context =>
                    {
                        var stripeClient = new StripeClient(StripeConfiguration.ApiKey, apiBase: StripeClient.DefaultApiBase);
                        return stripeClient;
                    }).As<IStripeClient>()
                .InstancePerLifetimeScope();
            builder.RegisterDecorator<StripeClientDecorator, IStripeClient>();

            builder.Register<IProductRegistry>(
                    ctx =>
                    {
                        var envChecker = ctx.Resolve<IDetermineCurrentEnvironment>();
                        if (envChecker.IsProduction())
                        {
                            return new ProductionProductRegistry();
                        }
                        else if (envChecker.IsStaging())
                        {
                            return new StagingProductRegistry();
                        }
                        else
                        {
                            return new TestProductRegistry();
                        }
                    })
                .InstancePerLifetimeScope();
        }
    }
}