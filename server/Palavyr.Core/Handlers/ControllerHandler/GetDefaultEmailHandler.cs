using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDefaultEmailHandler : IRequestHandler<GetDefaultEmailRequest, GetDefaultEmailResponse>
    {
        private readonly IEmailVerificationStatus emailVerificationStatus;
        private readonly IConfigurationEntityStore<Account> accountStore;

        public GetDefaultEmailHandler(
            IEmailVerificationStatus emailVerificationStatus,
            IConfigurationEntityStore<Account> accountStore)
        {
            this.emailVerificationStatus = emailVerificationStatus;
            this.accountStore = accountStore;
        }

        public async Task<GetDefaultEmailResponse> Handle(GetDefaultEmailRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            if (string.IsNullOrWhiteSpace(account.EmailAddress)) throw new Exception($"Default email for account id {account.AccountId} not found. Account corruption.");

            var verificationResponse = await emailVerificationStatus.GetVerificationResponse(account.EmailAddress);

            account.DefaultEmailIsVerified = verificationResponse.IsVerified();
            

            var response = AccountEmailSettingsResponse.CreateNew(
                account.EmailAddress,
                verificationResponse.IsVerified(),
                verificationResponse.IsPending()
            );
            return new GetDefaultEmailResponse(response);
        }
    }

    public class GetDefaultEmailResponse
    {
        public GetDefaultEmailResponse(AccountEmailSettingsResponse response) => Response = response;
        public AccountEmailSettingsResponse Response { get; set; }
    }

    public class GetDefaultEmailRequest : IRequest<GetDefaultEmailResponse>
    {
    }
}