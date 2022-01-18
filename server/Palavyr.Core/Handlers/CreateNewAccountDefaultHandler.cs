using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.Core.Handlers
{
    public class CreateNewAccountDefaultHandler : IRequestHandler<CreateNewAccountDefaultRequest, CreateNewAccountDefaultResponse>
    {
        private readonly IAccountSetupService setupService;

        public CreateNewAccountDefaultHandler(IAccountSetupService setupService)
        {
            this.setupService = setupService;
        }

        public async Task<CreateNewAccountDefaultResponse> Handle(CreateNewAccountDefaultRequest request, CancellationToken cancellationToken)
        {
            var credentials = await setupService.CreateNewAccountViaDefaultAsync(request.EmailAddress, request.Password, cancellationToken);
            return new CreateNewAccountDefaultResponse(credentials);
        }
    }

    public class CreateNewAccountDefaultResponse
    {
        public CreateNewAccountDefaultResponse(Credentials response) => Response = response;
        public Credentials Response { get; set; }
    }

    public class CreateNewAccountDefaultRequest : IRequest<CreateNewAccountDefaultResponse>
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}