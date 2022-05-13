using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetConfiguredIntentsHandler : IRequestHandler<GetConfiguredIntentsRequest, GetConfiguredIntentsResponse>
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly ICancellationTokenTransport cancellationTokenTransport;
        private readonly IMapToNew<Area, IntentResource> mapper;

        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;

        public GetConfiguredIntentsHandler(
            IEntityStore<Area> intentStore,
            ICancellationTokenTransport cancellationTokenTransport,
            IMapToNew<Area, IntentResource> mapper)
        {
            this.intentStore = intentStore;
            this.cancellationTokenTransport = cancellationTokenTransport;
            this.mapper = mapper;
        }

        public async Task<GetConfiguredIntentsResponse> Handle(GetConfiguredIntentsRequest request, CancellationToken cancellationToken)
        {
            var activeIntents = await intentStore.Query().Where(x => x.IsEnabled).ToListAsync(CancellationToken);
            var resource = await mapper.MapMany(activeIntents);

            return new GetConfiguredIntentsResponse(resource);
        }
    }

    public class GetConfiguredIntentsResponse
    {
        public GetConfiguredIntentsResponse(IEnumerable<IntentResource> response) => Response = response;
        public IEnumerable<IntentResource> Response { get; set; }
    }

    public class GetConfiguredIntentsRequest : IRequest<GetConfiguredIntentsResponse>
    {
    }
}