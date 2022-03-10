using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetStaticTableMetasHandler : IRequestHandler<GetStaticTableMetasRequest, GetStaticTableMetasResponse>
    {
        private readonly IEntityStore<StaticTablesMeta> staticTableMetaStore;
        private readonly ILogger<GetStaticTableMetasHandler> logger;

        public GetStaticTableMetasHandler(IEntityStore<StaticTablesMeta> staticTableMetaStore, ILogger<GetStaticTableMetasHandler> logger)
        {
            this.staticTableMetaStore = staticTableMetaStore;
            this.logger = logger;
        }

        public async Task<GetStaticTableMetasResponse> Handle(GetStaticTableMetasRequest request, CancellationToken cancellationToken)
        {
            var staticTables = await staticTableMetaStore.GetStaticTablesComplete(request.IntentId);
            return new GetStaticTableMetasResponse(staticTables);
        }
    }

    public class GetStaticTableMetasResponse
    {
        public GetStaticTableMetasResponse(List<StaticTablesMeta> response) => Response = response;
        public List<StaticTablesMeta> Response { get; set; }
    }

    public class GetStaticTableMetasRequest : IRequest<GetStaticTableMetasResponse>
    {
        public GetStaticTableMetasRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}