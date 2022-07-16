using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ResendConfirmationTokenHandler : IRequestHandler<ResendConfirmationTokenRequest, ResendConfirmationTokenResponse>
    {
        private readonly IEntityStore<EmailVerification> emailVerificationStore;
        private readonly IEmailVerificationService emailVerificationService;
        private readonly IAccountIdTransport accountIdTransport;

        public ResendConfirmationTokenHandler(
            IEntityStore<EmailVerification> emailVerificationStore,
            IEmailVerificationService emailVerificationService,
            IAccountIdTransport accountIdTransport)
        {
            this.emailVerificationStore = emailVerificationStore;
            this.emailVerificationService = emailVerificationService;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<ResendConfirmationTokenResponse> Handle(ResendConfirmationTokenRequest request, CancellationToken cancellationToken)
        {
            // delete any old records
            var maybeCurrentRecord = await emailVerificationStore.Get(request.EmailAddress, s => s.EmailAddress);
            if (maybeCurrentRecord != null)
            {
                await emailVerificationStore.Delete(maybeCurrentRecord);
            }

            // resend
            var result = await emailVerificationService.SendConfirmationTokenEmail(request.EmailAddress, cancellationToken);
            return new ResendConfirmationTokenResponse(result);
        }
    }

    public class ResendConfirmationTokenResponse
    {
        public ResendConfirmationTokenResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ResendConfirmationTokenRequest : IRequest<ResendConfirmationTokenResponse>
    {
        public string EmailAddress { get; set; }
    }
}