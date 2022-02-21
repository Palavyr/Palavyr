using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeServiceLocatorProvider
    {
        Stripe.BillingPortal.SessionService BillingSessionService { get; }
        Stripe.Checkout.SessionService CheckoutSessionService { get; }
        CustomerService CustomerService { get; set; }
        ProductService ProductService { get; set; }
        SubscriptionService SubscriptionService { get; set; }
    }

    public class StripeServiceLocatorProvider : IStripeServiceLocatorProvider
    {
        // I would normal NEVER do this, but stripe doesn't expose specific interfaces for its services
        public Stripe.BillingPortal.SessionService BillingSessionService { get; }
        public Stripe.Checkout.SessionService CheckoutSessionService { get; }
        public CustomerService CustomerService { get; set; }
        public ProductService ProductService { get; set; }
        public SubscriptionService SubscriptionService { get; set; }

        public StripeServiceLocatorProvider(
            Stripe.BillingPortal.SessionService billingSessionService,
            Stripe.Checkout.SessionService checkoutSessionService,
            CustomerService customerService,
            ProductService productService,
            SubscriptionService subscriptionService)
        {
            BillingSessionService = billingSessionService;
            CheckoutSessionService = checkoutSessionService;
            CustomerService = customerService;
            ProductService = productService;
            SubscriptionService = subscriptionService;
        }
    }
}