using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDefaultFallbackEmailTemplateHandler : IRequestHandler<GetDefaultFallbackEmailTemplateRequest, GetDefaultFallbackEmailTemplateResponse>
    {
        private readonly IAccountRepository accountRepository;

        public GetDefaultFallbackEmailTemplateHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<GetDefaultFallbackEmailTemplateResponse> Handle(GetDefaultFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
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