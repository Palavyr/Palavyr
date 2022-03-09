using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDefaultFallbackEmailSubjectHandler : IRequestHandler<GetDefaultFallbackEmailSubjectRequest, GetDefaultFallbackEmailSubjectResponse>
    {
        private readonly IConfigurationEntityStore<Account> accountStore;

        public GetDefaultFallbackEmailSubjectHandler(IConfigurationEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<GetDefaultFallbackEmailSubjectResponse> Handle(GetDefaultFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
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