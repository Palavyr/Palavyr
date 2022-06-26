using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateDynamicTableHandler : IRequestHandler<CreateDynamicTableRequest, CreateDynamicTableResponse>
    {
        private readonly IMapToNew<DynamicTableMeta, PricingStrategyTableMetaResource> mapper;
        private readonly IEntityStore<SelectOneFlat> selectOneFlatStore;
        private readonly IEntityStore<Area> intentStore;
        private readonly ILogger<CreateDynamicTableHandler> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public CreateDynamicTableHandler(
            IMapToNew<DynamicTableMeta, PricingStrategyTableMetaResource> mapper,
            IEntityStore<SelectOneFlat> selectOneFlatStore,
            IEntityStore<Area> intentStore,
            ILogger<CreateDynamicTableHandler> logger,
            IAccountIdTransport accountIdTransport)
        {
            this.mapper = mapper;
            this.selectOneFlatStore = selectOneFlatStore;
            this.intentStore = intentStore;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<CreateDynamicTableResponse> Handle(CreateDynamicTableRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.GetIntentComplete(request.IntentId);

            var dynamicTables = intent.DynamicTableMetas;

            var tableId = Guid.NewGuid().ToString();
            var tableTag = "Default-" + StaticGuidUtils.CreatePseudoRandomString(5);

            var newTableMeta = DynamicTableMeta.CreateNew(
                tableTag,
                PricingStrategyTableTypes.DefaultTable.PrettyName,
                PricingStrategyTableTypes.DefaultTable.TableType,
                tableId,
                request.IntentId,
                accountIdTransport.AccountId,
                UnitIds.Currency);

            dynamicTables.Add(newTableMeta);
            intent.DynamicTableMetas = dynamicTables;

            var defaultDynamicTable = new SelectOneFlat();
            var defaultTable = defaultDynamicTable.CreateTemplate(accountIdTransport.AccountId, request.IntentId, tableId);

            await selectOneFlatStore.Create(defaultTable);

            var resource = await mapper.Map(newTableMeta);
            // OH DEAR. I guess we aren't sending back some default values like the ID because we don't create the ID until we save the the thing and thats
            // on the damn way out
            return new CreateDynamicTableResponse(resource);
        }
    }

    public class CreateDynamicTableResponse
    {
        public CreateDynamicTableResponse(PricingStrategyTableMetaResource response) => Response = response;
        public PricingStrategyTableMetaResource Response { get; set; }
    }

    public class CreateDynamicTableRequest : IRequest<CreateDynamicTableResponse>
    {
        public CreateDynamicTableRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}