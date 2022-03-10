using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyDefaultFallbackEmailTemplateHandler : IRequestHandler<ModifyDefaultFallbackEmailTemplateRequest, ModifyDefaultFallbackEmailTemplateResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public ModifyDefaultFallbackEmailTemplateHandler(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<ModifyDefaultFallbackEmailTemplateResponse> Handle(ModifyDefaultFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            account.GeneralFallbackEmailTemplate = request.EmailTemplate;
            
            return new ModifyDefaultFallbackEmailTemplateResponse(account.GeneralFallbackEmailTemplate);
        }
    }

    public class ModifyDefaultFallbackEmailTemplateResponse
    {
        public ModifyDefaultFallbackEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyDefaultFallbackEmailTemplateRequest : IRequest<ModifyDefaultFallbackEmailTemplateResponse>
    {
        public string EmailTemplate { get; set; }
    }
}