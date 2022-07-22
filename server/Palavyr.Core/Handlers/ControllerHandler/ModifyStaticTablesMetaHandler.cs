using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyStaticTablesMetaHandler : IRequestHandler<ModifyStaticTablesMetaRequest, ModifyStaticTablesMetaResponse>
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEntityStore<StaticTablesMeta> staticTableMetaStore;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IMapToNew<StaticTablesMeta, StaticTableMetaResource> mapper;

        public ModifyStaticTablesMetaHandler(
            IEntityStore<Intent> intentStore,
            IEntityStore<StaticTablesMeta> staticTableMetaStore,
            IAccountIdTransport accountIdTransport,
            IMapToNew<StaticTablesMeta, StaticTableMetaResource> mapper )
        {
            this.intentStore = intentStore;
            this.staticTableMetaStore = staticTableMetaStore;
            this.accountIdTransport = accountIdTransport;
            this.mapper = mapper;
        }

        public async Task<ModifyStaticTablesMetaResponse> Handle(ModifyStaticTablesMetaRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.GetIntentComplete(request.IntentId);
            var staticTableMetas = intent.StaticTablesMetas.ToArray();
            await staticTableMetaStore.Delete(staticTableMetas);

            var clearedMetas = StaticTablesMeta.BindTemplateList(request.StaticTableMetaUpdate, accountIdTransport.AccountId);
            intent.StaticTablesMetas = clearedMetas;

            var resource = await mapper.MapMany(clearedMetas, cancellationToken);
            return new ModifyStaticTablesMetaResponse(resource);
        }
    }

    public class ModifyStaticTablesMetaResponse
    {
        public ModifyStaticTablesMetaResponse(IEnumerable<StaticTableMetaResource> response) => Response = response;
        public IEnumerable<StaticTableMetaResource> Response { get; set; }
    }

    public class ModifyStaticTablesMetaRequest : IRequest<ModifyStaticTablesMetaResponse>
    {
        public List<StaticTablesMeta> StaticTableMetaUpdate { get; set; }
        public string IntentId { get; set; }
    }
}