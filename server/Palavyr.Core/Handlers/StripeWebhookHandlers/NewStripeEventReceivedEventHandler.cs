using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class NewStripeEventReceivedEventHandler : IRequestHandler<NewStripeEventReceivedEvent, NewStripeEventReceivedEventResponse>
    {
        private readonly IEntityStore<StripeWebhookReceivedRecord> stripeWebhookStore;

        public NewStripeEventReceivedEventHandler(IEntityStore<StripeWebhookReceivedRecord> stripeWebhookStore)
        {
            this.stripeWebhookStore = stripeWebhookStore;
        }

        public async Task<NewStripeEventReceivedEventResponse> Handle(NewStripeEventReceivedEvent notification, CancellationToken cancellationToken)
        {
            var records = await stripeWebhookStore
                .RawReadonlyQuery()
                .Where(x => x.PayloadSignature == notification.Signature)
                .ToListAsync(cancellationToken);
            if (records is null)
            {
                throw new Exception("Couldn't find any records");
            }
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