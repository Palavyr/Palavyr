using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class NewStripeEventReceivedEventHandler : IRequestHandler<NewStripeEventReceivedEvent, NewStripeEventReceivedEventResponse>
    {
        private readonly IEntityStore<StripeWebhookRecord> stripeWebhookStore;

        public NewStripeEventReceivedEventHandler(IEntityStore<StripeWebhookRecord> stripeWebhookStore)
        {
            this.stripeWebhookStore = stripeWebhookStore;
        }

        public async Task<NewStripeEventReceivedEventResponse> Handle(NewStripeEventReceivedEvent notification, CancellationToken cancellationToken)
        {
            var records = await stripeWebhookStore.GetMany(notification.Signature, s => s.PayloadSignature);
            var exists = records.Count > 0;
            return new NewStripeEventReceivedEventResponse(shouldCancelProcessing: exists);
        }
    }

    public class NewStripeEventReceivedEventResponse
    {
        public bool ShouldCancelProcessing { get; set; }

        public NewStripeEventReceivedEventResponse(bool shouldCancelProcessing)
        {
            this.ShouldCancelProcessing = shouldCancelProcessing;
        }
    }

    public class NewStripeEventReceivedEvent : IRequest<NewStripeEventReceivedEventResponse>
    {
        public string Signature { get; set; }

        public NewStripeEventReceivedEvent(string signature)
        {
            this.Signature = signature;
        }
    }
}