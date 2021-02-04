using Autofac;
using Microsoft.Extensions.Configuration;
using Palavyr.API.Services.StripeServices;
using Palavyr.API.Services.StripeServices.StripeWebhookHandlers;
using Palavyr.Common.Constants;
using Stripe;

namespace Palavyr.API.Registration.Autofac
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
            StripeConfiguration.ApiKey = configuration.GetSection(ConfigSections.StripeKeySection).Value;
            StripeConfiguration.MaxNetworkRetries = stripeRetriesCount;

            builder.RegisterType<StripeWebhookAuthService>().AsSelf();
            builder.RegisterType<StripeEventWebhookService>().AsSelf();
            builder.RegisterType<StripeCustomerService>().AsSelf();
            builder.RegisterType<StripeSubscriptionService>().AsSelf();
            builder.RegisterType<StripeProductService>().AsSelf();

            builder.RegisterType<ProcessStripeSubscriptionUpdatedHandler>().AsSelf();
            builder.RegisterType<ProcessStripeSubscriptionDeletedHandler>().AsSelf();
            builder.RegisterType<ProcessStripeCheckoutSessionCompletedHandler>().AsSelf();
            builder.RegisterType<ProcessStripeInvoicePaidHandler>().AsSelf();
            builder.RegisterType<ProcessStripeInvoicePaymentFailedHandler>().AsSelf();
        }
    }
}