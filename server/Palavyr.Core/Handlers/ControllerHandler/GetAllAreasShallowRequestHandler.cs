using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAllAreasShallowRequestHandler : IRequestHandler<GetAllAreasRequest, GetAllAreasResponse>
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly ILogger<GetAllAreasShallowRequestHandler> logger;

        public GetAllAreasShallowRequestHandler(IEntityStore<Area> intentStore, ILogger<GetAllAreasShallowRequestHandler> logger)
        {
            this.intentStore = intentStore;
            this.logger = logger;
        }

        public async Task<GetAllAreasResponse> Handle(GetAllAreasRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Return all areas");
            var intents = await intentStore.GetMany(intentStore.AccountId, s => s.AreaIdentifier);
            return new GetAllAreasResponse(intents);
        }
    }

    public class GetAllAreasRequest : IRequest<GetAllAreasResponse>
    {
    }

    public class GetAllAreasResponse
    {
        public List<Area> Response { get; }

        public GetAllAreasResponse(List<Area> response)
        {
            Response = response;
        }
    }
}