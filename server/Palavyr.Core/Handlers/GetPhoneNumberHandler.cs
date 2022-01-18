using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetPhoneNumberHandler : IRequestHandler<GetPhoneNumberRequest, GetPhoneNumberResponse>
    {
        private readonly IAccountRepository accountRepository;

        public GetPhoneNumberHandler(
            IAccountRepository accountRepository,
            ILogger<GetPhoneNumberHandler> logger
        )
        {
            this.accountRepository = accountRepository;
        }

        public async Task<GetPhoneNumberResponse> Handle(GetPhoneNumberRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            var phoneDetails = PhoneDetails.Create(account.PhoneNumber, account.Locale);
            return new GetPhoneNumberResponse(phoneDetails);
        }
    }

    public class GetPhoneNumberResponse
    {
        public GetPhoneNumberResponse(PhoneDetails response) => Response = response;
        public PhoneDetails Response { get; set; }
    }

    public class GetPhoneNumberRequest : IRequest<GetPhoneNumberResponse>
    {
    }
}