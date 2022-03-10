using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDefaultFallbackEmailTemplateHandler : IRequestHandler<GetDefaultFallbackEmailTemplateRequest, GetDefaultFallbackEmailTemplateResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public GetDefaultFallbackEmailTemplateHandler(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<GetDefaultFallbackEmailTemplateResponse> Handle(GetDefaultFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            var currentDefaultEmailTemplate = account.GeneralFallbackEmailTemplate;
            return new GetDefaultFallbackEmailTemplateResponse(currentDefaultEmailTemplate);
        }
    }

    public class GetDefaultFallbackEmailTemplateResponse
    {
        public GetDefaultFallbackEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetDefaultFallbackEmailTemplateRequest : IRequest<GetDefaultFallbackEmailTemplateResponse>
    {
        public GetDefaultFallbackEmailTemplateRequest()
        {
        }
    }
}