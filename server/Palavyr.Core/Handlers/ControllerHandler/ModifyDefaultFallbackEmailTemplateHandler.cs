using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyDefaultFallbackEmailTemplateHandler : IRequestHandler<ModifyDefaultFallbackEmailTemplateRequest, ModifyDefaultFallbackEmailTemplateResponse>
    {
        private readonly IConfigurationEntityStore<Account> accountStore;

        public ModifyDefaultFallbackEmailTemplateHandler(IConfigurationEntityStore<Account> accountStore)
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