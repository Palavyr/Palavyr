using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class StripeEventReceivedEventHandler : INotificationHandler<StripeEventProcessedSuccessfullyEvent>
    {
        private readonly IAccountRepository accountRepository;

        public StripeEventReceivedEventHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        public async Task Handle(StripeEventProcessedSuccessfullyEvent notification, CancellationToken cancellationToken)
        {
            await accountRepository.AddStripeEvent(notification.Id, notification.Signature);
            await accountRepository.CommitChangesAsync();
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