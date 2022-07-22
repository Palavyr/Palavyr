using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class PasswordRequestHandler : IRequestHandler<PasswordRequestRequest, PasswordRequestResponse>
    {
        private readonly IEntityStore<UserSession> sessionStore;
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEntityStore<Account> accountStore;
        private readonly IRemoveStaleSessions removeStaleSessions;
        private readonly ISesEmail client;

        public PasswordRequestHandler(
            IEntityStore<UserSession> sessionStore,
            IEntityStore<Intent> intentStore,
            IEntityStore<Account> accountStore,
            IRemoveStaleSessions removeStaleSessions,
            ISesEmail client)
        {
            this.sessionStore = sessionStore;
            this.intentStore = intentStore;
            this.accountStore = accountStore;
            this.removeStaleSessions = removeStaleSessions;
            this.client = client;
        }

        public async Task<PasswordRequestResponse> Handle(PasswordRequestRequest request, CancellationToken cancellationToken)
        {
            var ambiguousMessage = "An email was sent to this address if an account for it exists.";

            var account = await accountStore.Get(request.EmailAddress, s => s.EmailAddress);
            if (account == null)
            {
                return new PasswordRequestResponse(new PasswordResetRequestResource(ambiguousMessage, false));
            }

            if (account.AccountType != AccountType.Default)
            {
                return new PasswordRequestResponse(new PasswordResetRequestResource("This account does use password sign-in.", false));
            }

            var token = string.Join("-", new[] { StaticGuidUtils.CreateNewId(), StaticGuidUtils.CreateNewId(), StaticGuidUtils.CreateNewId(), StaticGuidUtils.CreateNewId() }).Replace("-", "");
            var apiKey = account.ApiKey;

            await removeStaleSessions.CleanSessionDb(account.AccountId);
            var session = UserSession.CreateNew(token, accountStore.AccountId, account.ApiKey);
            await sessionStore.Create(session);

            var link = request.ResetPasswordLinkTemplate + token;

            var subject = "Reset your Palavyr Password";
            var htmlBody = $"<h2>Click on the following link to reset your password:</h2><p>{link}</p>";
            var textBody = $"Click on the following link to reset your password: {link}";
            var ok = await client.SendEmail(EmailConstants.PalavyrMainEmailAddress, request.EmailAddress, subject, htmlBody, textBody);
            if (!ok)
            {
                return new PasswordRequestResponse(new PasswordResetRequestResource("That doesn't seem to be a real email address. Maybe check your spelling?", false));
            }

            return new PasswordRequestResponse(new PasswordResetRequestResource(ambiguousMessage, true));
        }
    }

    public class PasswordResetRequestResource
    {
        public string Message { get; set; }
        public bool Status { get; set; }

        public PasswordResetRequestResource(string message, bool status)
        {
            Message = message;
            Status = status;
        }
    }

    public class PasswordRequestResponse
    {
        public PasswordRequestResponse(PasswordResetRequestResource requestResource) => RequestResource = requestResource;
        public PasswordResetRequestResource RequestResource { get; set; }
    }

    public class PasswordRequestRequest : IRequest<PasswordRequestResponse>
    {
        public string EmailAddress { get; set; }
        public string ResetPasswordLinkTemplate { get; set; }
    }
}