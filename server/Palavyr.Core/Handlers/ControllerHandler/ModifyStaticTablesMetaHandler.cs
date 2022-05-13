using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyStaticTablesMetaHandler : IRequestHandler<ModifyStaticTablesMetaRequest, ModifyStaticTablesMetaResponse>
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly IEntityStore<StaticTablesMeta> staticTableMetaStore;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IMapToNew<StaticTablesMeta, StaticTablesMetaResource> mapper;

        public ModifyStaticTablesMetaHandler(
            IEntityStore<Area> intentStore,
            IEntityStore<StaticTablesMeta> staticTableMetaStore,
            IAccountIdTransport accountIdTransport,
            IMapToNew<StaticTablesMeta, StaticTablesMetaResource> mapper )
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

            var resource = await mapper.MapMany(clearedMetas);
            return new ModifyStaticTablesMetaResponse(resource);
        }
    }

    public class ModifyStaticTablesMetaResponse
    {
        public ModifyStaticTablesMetaResponse(IEnumerable<StaticTablesMetaResource> response) => Response = response;
        public IEnumerable<StaticTablesMetaResource> Response { get; set; }
    }

    public class ModifyStaticTablesMetaRequest : IRequest<ModifyStaticTablesMetaResponse>
    {
        public List<StaticTablesMeta> StaticTableMetaUpdate { get; set; }
        public string IntentId { get; set; }
    }
}