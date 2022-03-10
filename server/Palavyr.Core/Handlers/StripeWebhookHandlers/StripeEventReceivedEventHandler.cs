using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class StripeEventReceivedEventHandler : INotificationHandler<StripeEventProcessedSuccessfullyEvent>
    {
        private readonly IEntityStore<StripeWebhookRecord> stripeWebhookStore;

        public StripeEventReceivedEventHandler(IEntityStore<StripeWebhookRecord> stripeWebhookStore)
        {
            this.stripeWebhookStore = stripeWebhookStore;
        }

        public async Task Handle(StripeEventProcessedSuccessfullyEvent notification, CancellationToken cancellationToken)
        {
            var newRecord = StripeWebhookRecord.CreateNewRecord(notification.Id, notification.Signature);
            await stripeWebhookStore.Create(newRecord);
        }
    }

    public class StripeEventProcessedSuccessfullyEvent : INotification
    {
        public readonly string Id;
        public readonly string Signature;

        public StripeEventProcessedSuccessfullyEvent(string id, string signature)
        {
            this.Id = id;
            this.Signature = signature;
        }
    }
}