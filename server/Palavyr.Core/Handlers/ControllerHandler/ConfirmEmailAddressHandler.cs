using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ConfirmEmailAddressHandler : IRequestHandler<ConfirmEmailAddressRequest, ConfirmEmailAddressResponse>
    {
        private readonly IEmailVerificationService emailVerificationService;

        public ConfirmEmailAddressHandler(IEmailVerificationService emailVerificationService)
        {
            this.emailVerificationService = emailVerificationService;
        }

        public async Task<ConfirmEmailAddressResponse> Handle(ConfirmEmailAddressRequest request, CancellationToken cancellationToken)
        {
            var confirmed = await emailVerificationService.ConfirmEmailAddressAsync(request.AuthToken.Trim(), cancellationToken);
            return new ConfirmEmailAddressResponse(confirmed);
        }
    }

    public class ConfirmEmailAddressResponse
    {
        public ConfirmEmailAddressResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ConfirmEmailAddressRequest : IRequest<ConfirmEmailAddressResponse>
    {
        public string AuthToken { get; set; }

        public ConfirmEmailAddressRequest(string authToken)
        {
            AuthToken = authToken;
        }
    }
}