using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyFallbackEmailSubjectHandler : IRequestHandler<ModifyFallbackEmailSubjectRequest, ModifyFallbackEmailSubjectResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public ModifyFallbackEmailSubjectHandler(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<ModifyFallbackEmailSubjectResponse> Handle(ModifyFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            account.GeneralFallbackSubject = request.Subject;
            
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