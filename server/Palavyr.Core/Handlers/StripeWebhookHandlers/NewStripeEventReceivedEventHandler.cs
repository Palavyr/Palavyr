using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class NewStripeEventReceivedEventHandler : IRequestHandler<NewStripeEventReceivedEvent, NewStripeEventReceivedEventResponse>
    {
        private readonly IAccountRepository accountRepository;

        public NewStripeEventReceivedEventHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<NewStripeEventReceivedEventResponse> Handle(NewStripeEventReceivedEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var exists = await accountRepository.SignedStripePayloadExists(notification.Signature);
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