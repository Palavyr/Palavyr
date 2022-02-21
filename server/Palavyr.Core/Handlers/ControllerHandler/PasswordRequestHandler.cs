using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class PasswordRequestHandler : IRequestHandler<PasswordRequestRequest, PasswordRequestResponse>
    {
        private readonly IAccountRepository accountRepository;
        private readonly ISesEmail client;

        public PasswordRequestHandler(
            IAccountRepository accountRepository,
            ISesEmail client)
        {
            this.accountRepository = accountRepository;
            this.client = client;
        }

        public async Task<PasswordRequestResponse> Handle(PasswordRequestRequest request, CancellationToken cancellationToken)
        {
            var ambiguousMessage = "An email was sent to this address if an account for it exists.";

            var account = await accountRepository.GetAccountByEmailAddressOrNull(request.EmailAddress);
            if (account == null)
            {
                return new PasswordRequestResponse(new ResetResponse(ambiguousMessage, false));
            }

            if (account.AccountType != AccountType.Default)
            {
                return new PasswordRequestResponse(new ResetResponse("This account does use password sign-in.", false));
            }

            var token = string.Join("-", new[] { StaticGuidUtils.CreateNewId(), StaticGuidUtils.CreateNewId(), StaticGuidUtils.CreateNewId(), StaticGuidUtils.CreateNewId() }).Replace("-", "");
            var apiKey = account.ApiKey;

            await accountRepository.CreateAndAddNewSession(token, apiKey);

            var link = request.ResetPasswordLinkTemplate + token;

            var subject = "Reset your Palavyr Password";
            var htmlBody = $"<h2>Click on the following link to reset your password:</h2><p>{link}</p>";
            var textBody = $"Click on the following link to reset your password: {link}";
            var ok = await client.SendEmail(EmailConstants.PalavyrMainEmailAddress, request.EmailAddress, subject, htmlBody, textBody);
            if (!ok)
            {
                return new PasswordRequestResponse(new ResetResponse("That doesn't seem to be a real email address. Maybe check your spelling?", false));
            }

            return new PasswordRequestResponse(new ResetResponse(ambiguousMessage, true));
        }
    }

    public class ResetResponse
    {
        public string Message { get; set; }
        public bool Status { get; set; }

        public ResetResponse(string message, bool status)
        {
            Message = message;
            Status = status;
        }
    }

    public class PasswordRequestResponse
    {
        public PasswordRequestResponse(ResetResponse response) => Response = response;
        public ResetResponse Response { get; set; }
    }

    public class PasswordRequestRequest : IRequest<PasswordRequestResponse>
    {
        public string EmailAddress { get; set; }
        public string ResetPasswordLinkTemplate { get; set; }
    }
}