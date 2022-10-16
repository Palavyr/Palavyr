using System;
using Autofac;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Configuration;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Services.StripeServices.Products;
using Stripe;

namespace Palavyr.API.Registration.Container
{
    public class StripeModule : Module
    {
        private readonly ConfigContainerServer config;

        public StripeModule(ConfigContainerServer config)
        {
            this.config = config;
        }

        private const int stripeRetriesCount = 3;

        protected override void Load(ContainerBuilder builder)
        {
            var runningEnvironment = new DetermineCurrentEnvironment(config);

            // var currentEnv = configuration.GetCurrentEnvironment();
            var stripeKey = config.StripeSecret;

            if (!runningEnvironment.IsProduction() && stripeKey.ToLowerInvariant().StartsWith("sk_live_"))
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
            builder.RegisterType<StripeWebhookAccountGetter>().As<IStripeWebhookAccountGetter>();
            builder.RegisterType<StripeSubscriptionSetter>().As<IStripeSubscriptionSetter>();

            var apiBase =
                runningEnvironment.IsDevelopment()
                    ? "http://host.docker.internal:12111"
                    : StripeClient.DefaultApiBase;
            var apiKey =
                runningEnvironment.IsDevelopment()
                    ? "sk_test_123"
                    : StripeConfiguration.ApiKey;

            builder.Register(_ => new StripeClient(apiKey, apiBase: apiBase)).As<IStripeClient>()
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