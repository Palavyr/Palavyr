using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers
{
    public class CreateAreaHandler : IRequestHandler<CreateAreaRequest, CreateAreaResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ILogger<CreateAreaHandler> logger;
        private readonly IHoldAnAccountId holdAnAccountId;

        public CreateAreaHandler(
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository,
            ILogger<CreateAreaHandler> logger,
            IHoldAnAccountId holdAnAccountId
        )
        {
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
            this.logger = logger;
            this.holdAnAccountId = holdAnAccountId;
        }

        public async Task<CreateAreaResponse> Handle(CreateAreaRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();

            var defaultEmail = account.EmailAddress;
            var isVerified = account.DefaultEmailIsVerified;

            var newArea = await configurationRepository.CreateAndAddNewArea(request.AreaName, defaultEmail, isVerified);
            await configurationRepository.CommitChangesAsync();
            return new CreateAreaResponse(newArea);
        }
    }
    
    public class CreateAreaRequest : IRequest<CreateAreaResponse>
    {
        public string AreaName { get; set; }
    }
    
    public class UpdateAreaNameRequest : CreateAreaRequest {} 

    public class CreateAreaResponse
    {
        public readonly Area Response;

        public CreateAreaResponse(Area response) => Response = response;
    }
}