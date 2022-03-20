using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetConfiguredIntentsHandler : IRequestHandler<GetConfiguredIntentsRequest, GetConfiguredIntentsResponse>
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly ICancellationTokenTransport cancellationTokenTransport;

        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;

        public GetConfiguredIntentsHandler(
            IEntityStore<Area> intentStore,
            ICancellationTokenTransport cancellationTokenTransport
        )
        {
            this.intentStore = intentStore;
            this.cancellationTokenTransport = cancellationTokenTransport;
        }

        public async Task<GetConfiguredIntentsResponse> Handle(GetConfiguredIntentsRequest request, CancellationToken cancellationToken)
        {
            var activeIntents = await intentStore.Query().Where(x => x.IsEnabled).ToListAsync(CancellationToken);
            return new GetConfiguredIntentsResponse(activeIntents);
        }
    }

    public class GetConfiguredIntentsResponse
    {
        public GetConfiguredIntentsResponse(List<Area> response) => Response = response;
        public List<Area> Response { get; set; }
    }

    public class GetConfiguredIntentsRequest : IRequest<GetConfiguredIntentsResponse>
    {
    }
}