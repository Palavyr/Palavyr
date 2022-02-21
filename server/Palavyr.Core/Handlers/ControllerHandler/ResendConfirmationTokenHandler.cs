using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ResendConfirmationTokenHandler : IRequestHandler<ResendConfirmationTokenRequest, ResendConfirmationTokenResponse>
    {
        private readonly IEmailVerificationService emailVerificationService;
        private readonly AccountsContext accountsContext;
        private readonly IHoldAnAccountId accountIdHolder;

        public ResendConfirmationTokenHandler(
            IEmailVerificationService emailVerificationService,
            AccountsContext accountsContext,
            IHoldAnAccountId accountIdHolder)
        {
            this.emailVerificationService = emailVerificationService;
            this.accountsContext = accountsContext;
            this.accountIdHolder = accountIdHolder;
        }

        public async Task<ResendConfirmationTokenResponse> Handle(ResendConfirmationTokenRequest request, CancellationToken cancellationToken)
        {
            // delete any old records
            var maybeCurrentRecord = accountsContext.EmailVerifications
                .SingleOrDefault(x => x.EmailAddress == request.EmailAddress && x.AccountId == accountIdHolder.AccountId);
            if (maybeCurrentRecord != null)
            {
                accountsContext.EmailVerifications.Remove(maybeCurrentRecord);
                await accountsContext.SaveChangesAsync(cancellationToken);
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