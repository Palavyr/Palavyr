using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class VerifyPasswordResetRequestHandler : IRequestHandler<VerifyPasswordResetRequestRequest, VerifyPasswordResetRequestResponse>
    {
        private readonly IEntityStore<UserSession> sessionStore;

        public VerifyPasswordResetRequestHandler(IEntityStore<UserSession> sessionStore)
        {
            this.sessionStore = sessionStore;
        }

        public async Task<VerifyPasswordResetRequestResponse> Handle(VerifyPasswordResetRequestRequest request, CancellationToken cancellationToken)
        {
            // the return from this is presented in the redirect page. 
            var session = await sessionStore.Get(request.Token, s => s.SessionId);

            if (session == null)
            {
                return new VerifyPasswordResetRequestResponse(new PasswordVerificationResource("Not Authorized.", false, ""));
            }

            if (sessionIsExpired(session.Expiration))
            {
                return new VerifyPasswordResetRequestResponse(new PasswordVerificationResource("Verification link has expired. Please resubmit a password change request.", false, ""));
            }

            return new VerifyPasswordResetRequestResponse(
                new PasswordVerificationResource(
                    "Successfully verified your request to reset your password.",
                    true,
                    session.ApiKey
                ));
        }

        private bool sessionIsExpired(DateTime expiration)
        {
            return expiration < DateTime.Now;
        }
    }

    public class VerifyPasswordResetRequestResponse
    {
        public VerifyPasswordResetRequestResponse(PasswordVerificationResource resource) => Resource = resource;
        public PasswordVerificationResource Resource { get; set; }
    }

    public class VerifyPasswordResetRequestRequest : IRequest<VerifyPasswordResetRequestResponse>
    {
        public VerifyPasswordResetRequestRequest(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
    }


    public class PasswordVerificationResource
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public string ApiKey { get; set; }

        public PasswordVerificationResource(string message, bool status, string apiKey)
        {
            Message = message;
            Status = status;
            ApiKey = apiKey;
        }
    }
}