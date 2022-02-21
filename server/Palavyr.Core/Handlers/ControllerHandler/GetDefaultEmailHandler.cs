using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDefaultEmailHandler : IRequestHandler<GetDefaultEmailRequest, GetDefaultEmailResponse>
    {
        private readonly IEmailVerificationStatus emailVerificationStatus;
        private readonly IAccountRepository accountRepository;

        public GetDefaultEmailHandler(
            IEmailVerificationStatus emailVerificationStatus,
            IAccountRepository accountRepository)
        {
            this.emailVerificationStatus = emailVerificationStatus;
            this.accountRepository = accountRepository;
        }

        public async Task<GetDefaultEmailResponse> Handle(GetDefaultEmailRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            if (string.IsNullOrWhiteSpace(account.EmailAddress)) throw new Exception($"Default email for account id {account.AccountId} not found. Account corruption.");

            var verificationResponse = await emailVerificationStatus.GetVerificationResponse(account.EmailAddress);

            account.DefaultEmailIsVerified = verificationResponse.IsVerified();
            await accountRepository.CommitChangesAsync();

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