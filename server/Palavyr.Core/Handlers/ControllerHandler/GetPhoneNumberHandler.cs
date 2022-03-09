using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetPhoneNumberHandler : IRequestHandler<GetPhoneNumberRequest, GetPhoneNumberResponse>
    {
        private readonly IConfigurationEntityStore<Account> accountStore;

        public GetPhoneNumberHandler(
            IConfigurationEntityStore<Account> accountStore,
            ILogger<GetPhoneNumberHandler> logger
        )
        {
            this.accountStore = accountStore;
        }

        public async Task<GetPhoneNumberResponse> Handle(GetPhoneNumberRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
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