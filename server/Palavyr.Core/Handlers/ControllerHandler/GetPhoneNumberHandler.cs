using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Resources.Responses;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetPhoneNumberHandler : IRequestHandler<GetPhoneNumberRequest, GetPhoneNumberResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public GetPhoneNumberHandler(
            IEntityStore<Account> accountStore,
            ILogger<GetPhoneNumberHandler> logger
        )
        {
            this.accountStore = accountStore;
        }

        public async Task<GetPhoneNumberResponse> Handle(GetPhoneNumberRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            var phoneDetails = PhoneDetailsResource.Create(account.PhoneNumber, account.Locale);
            return new GetPhoneNumberResponse(phoneDetails);
        }
    }

    public class GetPhoneNumberResponse
    {
        public GetPhoneNumberResponse(PhoneDetailsResource response) => Response = response;
        public PhoneDetailsResource Response { get; set; }
    }

    public class GetPhoneNumberRequest : IRequest<GetPhoneNumberResponse>
    {
    }
}