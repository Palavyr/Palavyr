using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetStripeCustomerIdHandler : IRequestHandler<GetStripeCustomerIdRequest, GetStripeCustomerIdResponse>
    {
        private readonly IAccountRepository accountRepository;

        public GetStripeCustomerIdHandler(
            ILogger<GetStripeCustomerIdHandler> logger,
            IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<GetStripeCustomerIdResponse> Handle(GetStripeCustomerIdRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            return new GetStripeCustomerIdResponse(account.StripeCustomerId);
        }
    }

    public class GetStripeCustomerIdResponse
    {
        public GetStripeCustomerIdResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetStripeCustomerIdRequest : IRequest<GetStripeCustomerIdResponse>
    {
    }
}