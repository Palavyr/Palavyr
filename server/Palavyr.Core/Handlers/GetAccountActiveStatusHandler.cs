using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetAccountActiveStatusHandler : IRequestHandler<GetAccountActiveStatusRequest, GetAccountActiveStatusResponse>
    {
        private readonly IAccountRepository accountRepository;
        private readonly ILogger<GetAccountActiveStatusHandler> logger;

        public GetAccountActiveStatusHandler(IAccountRepository accountRepository, ILogger<GetAccountActiveStatusHandler> logger)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
        }

        public async Task<GetAccountActiveStatusResponse> Handle(GetAccountActiveStatusRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Activation controller hit! Again!");
            var account = await accountRepository.GetAccount();
            var isActive = account.Active;
            return new GetAccountActiveStatusResponse(isActive);
        }
    }

    public class GetAccountActiveStatusResponse
    {
        public GetAccountActiveStatusResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetAccountActiveStatusRequest : IRequest<GetAccountActiveStatusResponse>
    {
    }
}