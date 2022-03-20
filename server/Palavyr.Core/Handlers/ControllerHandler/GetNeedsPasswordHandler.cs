using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetNeedsPasswordHandler : IRequestHandler<GetNeedsPasswordRequest, GetNeedsPasswordResponse>
    {
        private readonly IEntityStore<Account> accountStore;
        private static readonly int[] NeedsPassword = new[] { 0 };

        public GetNeedsPasswordHandler(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<GetNeedsPasswordResponse> Handle(GetNeedsPasswordRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            return new GetNeedsPasswordResponse(NeedsPassword.Contains((int)(account.AccountType)));
        }
    }

    public class GetNeedsPasswordResponse
    {
        public GetNeedsPasswordResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetNeedsPasswordRequest : IRequest<GetNeedsPasswordResponse>
    {
    }
}