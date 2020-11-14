using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Server.Domain.Accounts;
using Stripe;
using Subscription = Stripe.Subscription;

namespace Palavyr.API.Controllers.Payments
{
    [AllowAnonymous]
    [Route("api/payments")]
    [ApiController]
    public class PaymentsWebhook : BaseController
    {
        private static ILogger<PaymentsWebhook> _logger;
        private readonly IStripeClient _stripeClient = new StripeClient(StripeConfiguration.ApiKey);
        private IConfiguration _config;
        private readonly string _webhookKeySection = "Stripe:WebhookKey";
        private const string _planType = "Plantype";
        
        public PaymentsWebhook(
            IConfiguration configuration,
            ILogger<PaymentsWebhook> logger,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env
        ) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
            _config = configuration;
        }

        [AllowAnonymous]
        [HttpPost("payments-webhook")]
        public async Task<IActionResult> SubscriptionWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            Event stripeEvent;
            try
            {
                var webhookSecret = "whsec_j5S0uAwwTeQ5z5czAMDLqI1uZAw8D4JS";
                // var webhookSecret = _config.GetSection(_webhookKeySection);
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    webhookSecret
                );
            }
            catch (Exception e)
            {
                _logger.LogDebug($"Webhook Failed: {e.Message}");
                return BadRequest();
            }

            switch (stripeEvent.Type)
            {
                case Events.CheckoutSessionCompleted: //"checkout.session.completed":
                    // Payment is successful and the subscription is created.
                    // You should provision the subscription.

                    // get account via customer ID
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    var account =
                        AccountContext.Accounts.SingleOrDefault(row => row.StripeCustomerId == session.CustomerId);
                    if (account == null)
                        throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");

                    // Get subscription details
                    var subscriptionService = new SubscriptionService(_stripeClient);
                    Subscription subscription;
                    try
                    {
                        subscription = await subscriptionService.GetAsync(session.SubscriptionId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug($"EXCEPTION: {ex.Message}");
                        throw new Exception();
                    }
                    var priceDetails = subscription.Items.Data[0].Price;
                    var productId = priceDetails.ProductId;
                    
                    var produceService = new ProductService(_stripeClient);
                    Product product;
                    try
                    {
                        product = await produceService.GetAsync(productId);
                    }
                    catch (StripeException ex)
                    {
                        _logger.LogDebug($"Could not find product: {ex.Message}");
                        throw new Exception("Exception on finding price details");
                    }
                    
                    var planFound = product.Metadata.TryGetValue(_planType, out var planType);
                    if (!planFound) throw new Exception("Plan Type not found in the subscription data!");
                    var paymentInterval = priceDetails.Recurring.Interval;

                    UserAccount.PlanTypeEnum planEnum;
                    switch (planType)
                    {
                        case (UserAccount.PlanTypes.Premium):
                            planEnum = UserAccount.PlanTypeEnum.Premium;
                            break;
                        case (UserAccount.PlanTypes.Pro):
                            planEnum = UserAccount.PlanTypeEnum.Pro;
                            break;
                        default:
                            planEnum = UserAccount.PlanTypeEnum.Free;
                            break;
                    }

                    UserAccount.PaymentIntervalEnum paymentIntervalEnum;
                    switch (paymentInterval)
                    {
                        case (UserAccount.PaymentIntervals.Month):
                            paymentIntervalEnum = UserAccount.PaymentIntervalEnum.Month;
                            break;
                        case (UserAccount.PaymentIntervals.Year):
                            paymentIntervalEnum = UserAccount.PaymentIntervalEnum.Year;
                            break;
                        default:
                            throw new Exception("Payment interval could not be determined");
                    }
                    
                    account.PlanType = planEnum;
                    account.HasUpgraded = true;
                    account.PaymentInterval = paymentIntervalEnum;
                    await AccountContext.SaveChangesAsync();
                    break;

                
                case Events.InvoicePaid: //"invoice.paid":
                    // Continue to provision the subscription as payments continue to be made.
                    // Store the status in your database and check when a user accesses your service.
                    // This approach helps you avoid hitting rate limits.

                    // TODO: Update the subscription database perhaps to record the account payment to display later in the account settings.
                    // TODO: Send an email thanking the user a fulfilled invoice and thank them for their ongoing business.
                    break;

                case Events.InvoicePaymentFailed: //"invoice.payment_failed":
                    // The payment failed or the customer does not have a valid payment method.
                    // The subscription becomes past_due. Notify your customer and send them to the
                    // customer portal to update their payment information.
                    
                    // TODO: Request that the user update their payment information
                    // TODO: Let them know how long until service is shut off.

                    break;
                    
                
                default:
                    _logger.LogDebug($"Event Type not recognized: {stripeEvent.Type}");
                    break;
            }

            return Ok();
        }
    }
}