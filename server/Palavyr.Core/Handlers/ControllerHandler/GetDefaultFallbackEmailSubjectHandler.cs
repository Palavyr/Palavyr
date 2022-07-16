using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDefaultFallbackEmailSubjectHandler : IRequestHandler<GetDefaultFallbackEmailSubjectRequest, GetDefaultFallbackEmailSubjectResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public GetDefaultFallbackEmailSubjectHandler(IEntityStore<Account> accountStore)
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