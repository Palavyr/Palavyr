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
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;
using Shouldly;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateDynamicTableHandler<T, TR, TCompiler>
        : IRequestHandler<CreateDynamicTableRequest<T, TR, TCompiler>, CreateDynamicTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, ITable, new()
        where TR : class, IPricingStrategyTableRowResource
        where TCompiler : class, IPricingStrategyTableCompiler
    {
        private readonly IMapToNew<DynamicTableMeta, PricingStrategyTableMetaResource> mapper;
        private readonly IEntityStore<SelectOneFlat> selectOneFlatStore;
        private readonly IEntityStore<Area> intentStore;
        private readonly ILogger<CreateDynamicTableHandler<T, TR, TCompiler>> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public CreateDynamicTableHandler(
            IMapToNew<DynamicTableMeta, PricingStrategyTableMetaResource> mapper,
            IEntityStore<SelectOneFlat> selectOneFlatStore,
            IEntityStore<Area> intentStore,
            ILogger<CreateDynamicTableHandler<T, TR, TCompiler>> logger,
            IAccountIdTransport accountIdTransport)
        {
            this.mapper = mapper;
            this.selectOneFlatStore = selectOneFlatStore;
            this.intentStore = intentStore;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<CreateDynamicTableResponse<TR>> Handle(CreateDynamicTableRequest<T, TR, TCompiler> request, CancellationToken cancellationToken)
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

            
            
            Type defaultType = typeof(TR);
            if (defaultType == typeof(SelectOneFlatResource))
            {
            }
            else if (defaultType == typeof(CategoryNestedThresholdResource))
            {
            }
            else if (defaultType == typeof(TwoNestedCategoryResource))
            {
            }
            else if (defaultType == typeof(BasicThresholdResource))
            {
            }
            else if (defaultType == typeof(PercentOfThresholdResource))
            {
            }
            else
            {
                throw new Exception("Pricing ")
            }
            var defaultDynamicTable = new SelectOneFlat();
            
            // TODO: Move the create template call perhaps to another class. I need to be able to resolve this type generically so I can call this method. I'll
            // need a generic interface for that as well - another like IMapToNew kinda thing..
            var defaultTable = defaultDynamicTable.CreateTemplate(accountIdTransport.AccountId, request.IntentId, tableId);

            await selectOneFlatStore.Create(defaultTable);

            var resource = await mapper.Map(newTableMeta);

            return new CreateDynamicTableResponse<TR>(resource);
        }
    }

    public class CreateDynamicTableResponse<TR> where TR : IPricingStrategyTableRowResource
    {
        public CreateDynamicTableResponse(PricingStrategyTableMetaResource response) => Response = response;
        public PricingStrategyTableMetaResource Response { get; set; }
    }

    public class CreateDynamicTableRequest<T, TR, TCompiler>
        : IRequest<CreateDynamicTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, new()
        where TR : IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
    {
        public CreateDynamicTableRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}