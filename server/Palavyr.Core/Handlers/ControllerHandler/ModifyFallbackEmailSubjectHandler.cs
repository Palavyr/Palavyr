using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyFallbackEmailSubjectHandler : IRequestHandler<ModifyFallbackEmailSubjectRequest, ModifyFallbackEmailSubjectResponse>
    {
        private readonly IAccountRepository accountRepository;

        public ModifyFallbackEmailSubjectHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<ModifyFallbackEmailSubjectResponse> Handle(ModifyFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            account.GeneralFallbackSubject = request.Subject;
            await accountRepository.CommitChangesAsync();
            return new ModifyFallbackEmailSubjectResponse(account.GeneralFallbackSubject);
        }
    }

    public class ModifyFallbackEmailSubjectResponse
    {
        public ModifyFallbackEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyFallbackEmailSubjectRequest : IRequest<ModifyFallbackEmailSubjectResponse>
    {
        public string Subject { get; set; }
    }
}