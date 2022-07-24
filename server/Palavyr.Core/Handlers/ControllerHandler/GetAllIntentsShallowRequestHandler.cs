using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAllIntentsShallowRequestHandler : IRequestHandler<GetAllIntentsRequest, GetAllIntentsResponse>
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly ILogger<GetAllIntentsShallowRequestHandler> logger;
        private readonly IMapToNew<Intent, IntentResource> mapper;

        public GetAllIntentsShallowRequestHandler(IEntityStore<Intent> intentStore, ILogger<GetAllIntentsShallowRequestHandler> logger, IMapToNew<Intent, IntentResource> mapper)
        {
            this.intentStore = intentStore;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<GetAllIntentsResponse> Handle(GetAllIntentsRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Return all intents");
            var allIntents = await intentStore.GetAllIntentsComplete();
            var resource = await mapper.MapMany(allIntents, cancellationToken);
            return new GetAllIntentsResponse(resource);
        }
    }

    public class GetAllIntentsRequest : IRequest<GetAllIntentsResponse>
    {
        public const string Route = "intents";
    }

    public class GetAllIntentsResponse
    {
        public GetAllIntentsResponse(IEnumerable<IntentResource> response)
        {
            Response = response;
        }

        public IEnumerable<IntentResource> Response { get; }
    }
}