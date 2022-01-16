﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class VerifyPasswordResetRequestHandler : IRequestHandler<VerifyPasswordResetRequestRequest, VerifyPasswordResetRequestResponse>
    {
        private readonly IAccountRepository accountRepository;

        public VerifyPasswordResetRequestHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<VerifyPasswordResetRequestResponse> Handle(VerifyPasswordResetRequestRequest request, CancellationToken cancellationToken)
        {
            // the return from this is presented in the redirect page. 
            var session = await accountRepository.GetSessionOrNull(request.Token);
            if (session == null)
            {
                return new VerifyPasswordResetRequestResponse(new VerificationResponse("Not Authorized.", false, ""));
            }

            if (sessionIsExpired(session.Expiration))
            {
                return new VerifyPasswordResetRequestResponse(new VerificationResponse("Verification link has expired. Please resubmit a password change request.", false, ""));
            }

            return new VerifyPasswordResetRequestResponse(
                new VerificationResponse(
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
        public VerifyPasswordResetRequestResponse(VerificationResponse response) => Response = response;
        public VerificationResponse Response { get; set; }
    }

    public class VerifyPasswordResetRequestRequest : IRequest<VerifyPasswordResetRequestResponse>
    {
        public VerifyPasswordResetRequestRequest(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
    }


    public class VerificationResponse
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public string ApiKey { get; set; }

        public VerificationResponse(string message, bool status, string apiKey)
        {
            Message = message;
            Status = status;
            ApiKey = apiKey;
        }
    }
}