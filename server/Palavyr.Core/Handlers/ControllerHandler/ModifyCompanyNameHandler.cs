using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyCompanyNameHandler : IRequestHandler<ModifyCompanyNameRequest, ModifyCompanyNameResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public ModifyCompanyNameHandler(IEntityStore<Account> accountStore, ILogger<ModifyCompanyNameHandler> logger)
        {
            this.accountStore = accountStore;
        }

        public async Task<ModifyCompanyNameResponse> Handle(ModifyCompanyNameRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            account.CompanyName = request.CompanyName;
            
            return new ModifyCompanyNameResponse(account.CompanyName);
        }
    }

    public class ModifyCompanyNameResponse
    {
        public ModifyCompanyNameResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyCompanyNameRequest : IRequest<ModifyCompanyNameResponse>
    {
        public string CompanyName { get; set; }
    }
}
