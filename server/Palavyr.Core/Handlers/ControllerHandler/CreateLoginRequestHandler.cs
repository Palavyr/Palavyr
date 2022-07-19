using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.Core.Handlers.ControllerHandler
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
            logger.LogDebug("Login Request Received");
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
        public CreateLoginRequestResponse(CredentialsResource response) => Response = response;
        public CredentialsResource Response { get; set; }
    }

    public class CreateLoginRequest : IRequest<CreateLoginRequestResponse>
    {
        [Required]
        public string EmailAddress { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}