using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetResponseConfigurationHandler : IRequestHandler<GetResponseConfigurationRequest, GetResponseConfigurationResponse>
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly IMapToNew<Area, IntentResource> mapper;

        public GetResponseConfigurationHandler(IEntityStore<Area> intentStore, IMapToNew<Area, IntentResource> mapper)
        {
            this.intentStore = intentStore;
            this.mapper = mapper;
        }

        public async Task<GetResponseConfigurationResponse> Handle(GetResponseConfigurationRequest request, CancellationToken cancellationToken)
        {
            var areaWithAllData = await intentStore.GetIntentComplete(request.IntentId);
            var resource = await mapper.Map(areaWithAllData);
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
        public GetResponseConfigurationRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}