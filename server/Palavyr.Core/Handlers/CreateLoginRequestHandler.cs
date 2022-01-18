using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.Core.Handlers
{
    public class CreateLoginRequestHandler : IRequestHandler<CreateLoginRequest, CreateLoginRequestResponse>
    {
        private readonly IAuthService authService;
        private readonly ILogger<CreateLoginRequestHandler> logger;

        public CreateLoginRequestHandler(IAuthService authService, ILogger<CreateLoginRequestHandler> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }

        public async Task<CreateLoginRequestResponse> Handle(CreateLoginRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Login Request Received.");
            try
            {
                var credentials = await authService.PerformLoginAction(request);
                return new CreateLoginRequestResponse(credentials);
            }
            catch (Exception ex)
            {
                logger.LogDebug("****************************");
                logger.LogDebug(ex.Message);
                throw;
            }
        }
    }

    public class CreateLoginRequestResponse
    {
        public CreateLoginRequestResponse(Credentials response) => Response = response;
        public Credentials Response { get; set; }
    }

    public class CreateLoginRequest : IRequest<CreateLoginRequestResponse>
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string SessionToken { get; set; }

        public string OldPassword { get; set; }
        public string PhoneNumber { get; set; }
    }
}