using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateDynamicTableHandler : IRequestHandler<CreateDynamicTableRequest, CreateDynamicTableResponse>
    {
        private readonly IConfigurationEntityStore<SelectOneFlat> selectOneFlatStore;
        private readonly IConfigurationEntityStore<Area> intentStore;
        private readonly ILogger<CreateDynamicTableHandler> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public CreateDynamicTableHandler(
            IConfigurationEntityStore<SelectOneFlat> selectOneFlatStore,
            IConfigurationEntityStore<Area> intentStore,
            ILogger<CreateDynamicTableHandler> logger,
            IAccountIdTransport accountIdTransport)
        {
            this.selectOneFlatStore = selectOneFlatStore;
            this.intentStore = intentStore;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<CreateDynamicTableResponse> Handle(CreateDynamicTableRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.GetIntentComplete(request.IntentId);

            var dynamicTables = intent.DynamicTableMetas.ToList();

            var tableId = Guid.NewGuid().ToString();
            var tableTag = "Default-" + StaticGuidUtils.CreatePseudoRandomString(5);

            var newTableMeta = DynamicTableMeta.CreateNew(
                tableTag,
                DynamicTableTypes.DefaultTable.PrettyName,
                DynamicTableTypes.DefaultTable.TableType,
                tableId,
                request.IntentId,
                accountIdTransport.AccountId,
                UnitIds.Currency);

            dynamicTables.Add(newTableMeta);
            intent.DynamicTableMetas = dynamicTables;

            var defaultDynamicTable = new SelectOneFlat();
            var defaultTable = defaultDynamicTable.CreateTemplate(accountIdTransport.AccountId, request.IntentId, tableId);

            await selectOneFlatStore.Create(defaultTable);
            return new CreateDynamicTableResponse(newTableMeta);
        }
    }

    public class CreateDynamicTableResponse
    {
        public CreateDynamicTableResponse(DynamicTableMeta response) => Response = response;
        public DynamicTableMeta Response { get; set; }
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