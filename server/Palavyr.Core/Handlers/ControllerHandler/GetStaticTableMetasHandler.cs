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
    public class GetStaticTableMetasHandler : IRequestHandler<GetStaticTableMetasRequest, GetStaticTableMetasResponse>
    {
        private readonly IEntityStore<StaticTablesMeta> staticTableMetaStore;
        private readonly ILogger<GetStaticTableMetasHandler> logger;
        private readonly IMapToNew<StaticTablesMeta, StaticTableMetaResource> mapper;

        public GetStaticTableMetasHandler(IEntityStore<StaticTablesMeta> staticTableMetaStore, ILogger<GetStaticTableMetasHandler> logger, IMapToNew<StaticTablesMeta, StaticTableMetaResource> mapper)
        {
            this.staticTableMetaStore = staticTableMetaStore;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<GetStaticTableMetasResponse> Handle(GetStaticTableMetasRequest request, CancellationToken cancellationToken)
        {
            var staticTables = await staticTableMetaStore.GetStaticTablesComplete(request.IntentId);

            var resource = await mapper.MapMany(staticTables, cancellationToken);
            return new GetStaticTableMetasResponse(resource);
        }
    }

    public class GetStaticTableMetasResponse
    {
        public GetStaticTableMetasResponse(IEnumerable<StaticTableMetaResource> response) => Response = response;
        public IEnumerable<StaticTableMetaResource> Response { get; set; }
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