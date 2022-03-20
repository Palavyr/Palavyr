using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Responses;
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
            var credentials = await setupService.CreateNewAccountViaDefaultAsync(request.EmailAddress, request.Password, cancellationToken);
            return new CreateNewAccountResponse(credentials);
        }
    }

    public class CreateNewAccountResponse
    {
        public CreateNewAccountResponse(Credentials response) => Response = response;
        public Credentials Response { get; set; }
    }

    public class CreateNewAccountRequest : IRequest<CreateNewAccountResponse>
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}