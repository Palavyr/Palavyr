using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateNewAccountHandler : IRequestHandler<CreateNewAccountRequest, CreateNewAccountResponse>
    {
        private readonly IAccountSetupService setupService;

        public CreateNewAccountHandler(IAccountSetupService accountSetupService)
        {
            this.setupService = accountSetupService;
        }

        public async Task<CreateNewAccountResponse> Handle(CreateNewAccountRequest request, CancellationToken cancellationToken)
        {
            var credentials = await setupService.CreateNewAccount(request.EmailAddress, request.Password, cancellationToken);
            return new CreateNewAccountResponse(credentials);
        }
    }

    public class CreateNewAccountResponse
    {
        public CreateNewAccountResponse(CredentialsResource response) => Response = response;
        public CredentialsResource Response { get; set; }
    }

    public class CreateNewAccountRequest : IRequest<CreateNewAccountResponse>
    {
        public const string Route = "account/create/default";


        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}