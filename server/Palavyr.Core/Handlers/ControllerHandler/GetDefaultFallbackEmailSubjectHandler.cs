using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDefaultFallbackEmailSubjectHandler : IRequestHandler<GetDefaultFallbackEmailSubjectRequest, GetDefaultFallbackEmailSubjectResponse>
    {
        private readonly IAccountRepository accountRepository;

        public GetDefaultFallbackEmailSubjectHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<GetDefaultFallbackEmailSubjectResponse> Handle(GetDefaultFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            var subject = account.GeneralFallbackSubject;
            return new GetDefaultFallbackEmailSubjectResponse(subject);
        }
    }

    public class GetDefaultFallbackEmailSubjectResponse
    {
        public GetDefaultFallbackEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetDefaultFallbackEmailSubjectRequest : IRequest<GetDefaultFallbackEmailSubjectResponse>
    {
    }
}