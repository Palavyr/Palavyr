using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyPhoneNumberHandler : IRequestHandler<ModifyPhoneNumberRequest, ModifyPhoneNumberResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public ModifyPhoneNumberHandler(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<ModifyPhoneNumberResponse> Handle(ModifyPhoneNumberRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            account.PhoneNumber = request.PhoneNumber ?? "";
            
            return new ModifyPhoneNumberResponse(account.PhoneNumber);
        }
    }

    public class ModifyPhoneNumberResponse
    {
        public ModifyPhoneNumberResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyPhoneNumberRequest : IRequest<ModifyPhoneNumberResponse>
    {
        public string PhoneNumber { get; set; }
    }
}