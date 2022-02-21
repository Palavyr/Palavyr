using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.StripeServices;
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

            builder.RegisterType<StripeWebhookAuthService>().AsSelf();
            builder.RegisterType<StripeEventWebhookRoutingService>().As<IStripeEventWebhookRoutingService>();
            builder.RegisterType<StripeCustomerService>().AsSelf();
            builder.RegisterType<StripeSubscriptionService>().As<IStripeSubscriptionService>();
            builder.RegisterType<StripeProductService>().AsSelf();
            builder.RegisterType<StripeCheckoutService>().AsSelf();

            builder.RegisterType<StripeCustomerManagementPortalService>().As<IStripeCustomerManagementPortalService>();

            builder.Register(
                    context =>
                    {
                        var determineCurrentEnvironment = new DetermineCurrentEnvironment(configuration);
                        var baseUri = determineCurrentEnvironment.IsDevelopment() ? "https://localhost:12111" : StripeClient.DefaultApiBase;
                        var stripeClient = new StripeClient(StripeConfiguration.ApiKey, apiBase: baseUri);
                        return stripeClient;
                    }).As<IStripeClient>()
                .InstancePerLifetimeScope();


            builder.Register(
                    context =>
                    {
                        var client = context.Resolve<IStripeClient>();

                        var billingSessionService = new Stripe.BillingPortal.SessionService(client);
                        var checkSessionService = new Stripe.Checkout.SessionService(client);
                        var customerService = new CustomerService(client);
                        var productService = new ProductService(client);
                        var subscriptionService = new SubscriptionService(client);

                        var provider = new StripeServiceLocatorProvider(
                            billingSessionService,
                            checkSessionService,
                            customerService,
                            productService,
                            subscriptionService);

                        return provider;
                    })
                .As<IStripeServiceLocatorProvider>();


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