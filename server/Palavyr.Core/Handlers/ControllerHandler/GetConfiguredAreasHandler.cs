using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetConfiguredIntentsHandler : IRequestHandler<GetConfiguredIntentsRequest, GetConfiguredIntentsResponse>
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly ICancellationTokenTransport cancellationTokenTransport;
        private readonly IMapToNew<Intent, IntentResource> mapper;

        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;

        public GetConfiguredIntentsHandler(
            IEntityStore<Intent> intentStore,
            ICancellationTokenTransport cancellationTokenTransport,
            IMapToNew<Intent, IntentResource> mapper)
        {
            this.intentStore = intentStore;
            this.cancellationTokenTransport = cancellationTokenTransport;
            this.mapper = mapper;
        }

        public async Task<GetConfiguredIntentsResponse> Handle(GetConfiguredIntentsRequest request, CancellationToken cancellationToken)
        {
            var activeIntents = await intentStore.Query().Where(x => x.IsEnabled).ToListAsync(CancellationToken);
            var resource = await mapper.MapMany(activeIntents, cancellationToken);

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