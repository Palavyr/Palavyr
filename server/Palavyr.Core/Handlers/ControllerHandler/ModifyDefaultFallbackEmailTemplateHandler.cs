using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyDefaultFallbackEmailTemplateHandler : IRequestHandler<ModifyDefaultFallbackEmailTemplateRequest, ModifyDefaultFallbackEmailTemplateResponse>
    {
        private readonly IAccountRepository accountRepository;

        public ModifyDefaultFallbackEmailTemplateHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<ModifyDefaultFallbackEmailTemplateResponse> Handle(ModifyDefaultFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            account.GeneralFallbackEmailTemplate = request.EmailTemplate;
            await accountRepository.CommitChangesAsync();
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