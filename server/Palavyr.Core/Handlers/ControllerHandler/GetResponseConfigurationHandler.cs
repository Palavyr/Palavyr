using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetResponseConfigurationHandler : IRequestHandler<GetResponseConfigurationRequest, GetResponseConfigurationResponse>
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly IMapToNew<Intent, IntentResource> mapper;

        public GetResponseConfigurationHandler(IEntityStore<Intent> intentStore, IMapToNew<Intent, IntentResource> mapper)
        {
            this.intentStore = intentStore;
            this.mapper = mapper;
        }

        public async Task<GetResponseConfigurationResponse> Handle(GetResponseConfigurationRequest request, CancellationToken cancellationToken)
        {
            var intentDeep = await intentStore.GetIntentComplete(request.IntentId);
            var resource = await mapper.Map(intentDeep, cancellationToken);
            return new GetResponseConfigurationResponse(resource);
        }
    }

    public class GetResponseConfigurationResponse
    {
        public GetResponseConfigurationResponse(IntentResource response) => Response = response;
        public IntentResource Response { get; set; }
    }

    public class GetResponseConfigurationRequest : IRequest<GetResponseConfigurationResponse>
    {
        public const string Route = "response/configuration/{intentId}";

        public GetResponseConfigurationRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}