using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAllAreasShallowRequestHandler : IRequestHandler<GetAllAreasRequest, GetAllAreasResponse>
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly ILogger<GetAllAreasShallowRequestHandler> logger;
        private readonly IMapToNew<Area, IntentResource> mapper;

        public GetAllAreasShallowRequestHandler(IEntityStore<Area> intentStore, ILogger<GetAllAreasShallowRequestHandler> logger, IMapToNew<Area, IntentResource> mapper)
        {
            this.intentStore = intentStore;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<GetAllAreasResponse> Handle(GetAllAreasRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Return all areas");
            var intents = await intentStore.GetMany(intentStore.AccountId, s => s.AccountId);
            var resource = await mapper.MapMany(intents);
            return new GetAllAreasResponse(resource);
        }
    }

    public class GetAllAreasRequest : IRequest<GetAllAreasResponse>
    {
    }

    public class GetAllAreasResponse
    {
        public GetAllAreasResponse(IEnumerable<IntentResource> response)
        {
            Response = response;
        }

        public IEnumerable<IntentResource> Response { get; }
    }
}