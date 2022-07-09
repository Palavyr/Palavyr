using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreatePricingStrategyTableHandler<T, TR, TCompiler>
        : IRequestHandler<CreatePricingStrategyTableRequest<T, TR, TCompiler>, CreatePricingStrategyTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, ITable, new()
        where TR : class, IPricingStrategyTableRowResource
        where TCompiler : class, IPricingStrategyTableCompiler
    {
        private readonly IMapToNew<DynamicTableMeta, PricingStrategyTableMetaResource> mapper;
        private readonly IEntityStore<T> pricingStrategyStore;
        private readonly IEntityStore<Area> intentStore;
        private readonly ILogger<CreatePricingStrategyTableHandler<T, TR, TCompiler>> logger;
        private readonly IPricingStrategyTemplateCreator<T> templateCreator;
        private readonly IAccountIdTransport accountIdTransport;

        public CreatePricingStrategyTableHandler(
            IMapToNew<DynamicTableMeta, PricingStrategyTableMetaResource> mapper,
            IEntityStore<T> pricingStrategyStore,
            IEntityStore<Area> intentStore,
            ILogger<CreatePricingStrategyTableHandler<T, TR, TCompiler>> logger,
            IPricingStrategyTemplateCreator<T> templateCreator,
            IAccountIdTransport accountIdTransport)
        {
            this.mapper = mapper;
            this.pricingStrategyStore = pricingStrategyStore;
            this.intentStore = intentStore;
            this.logger = logger;
            this.templateCreator = templateCreator;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<CreatePricingStrategyTableResponse<TR>> Handle(CreatePricingStrategyTableRequest<T, TR, TCompiler> request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.GetIntentComplete(request.IntentId);

            var dynamicTables = intent.DynamicTableMetas;

            var tableId = Guid.NewGuid().ToString();
            var tableTag = "Default-" + StaticGuidUtils.CreatePseudoRandomString(5);

            var newTableMeta = DynamicTableMeta.CreateNew(
                tableTag,
                templateCreator.GetPrettyName(),
                templateCreator.GetTableType(),
                tableId,
                request.IntentId,
                accountIdTransport.AccountId,
                UnitIds.Currency);

            dynamicTables.Add(newTableMeta);
            intent.DynamicTableMetas = dynamicTables;

            var template = templateCreator.CreateTemplate(request.IntentId, tableId);
            await pricingStrategyStore.Create(template);

            var resource = await mapper.Map(newTableMeta);

            return new CreatePricingStrategyTableResponse<TR>(resource);
        }
    }

    public interface IPricingStrategyTemplateCreator<TPricingStrategy>
        where TPricingStrategy : class, IPricingStrategyTable<TPricingStrategy>, IEntity
    {
        TPricingStrategy CreateTemplate(string intentId, string tableId);
        string GetPrettyName();
        string GetTableType();
    }

    public class PricingStrategyTemplateCreator<TPricingStrategy>
        : IPricingStrategyTemplateCreator<TPricingStrategy> where TPricingStrategy
        : class, IPricingStrategyTable<TPricingStrategy>, IEntity
    {
        private readonly IAccountIdTransport accountIdTransport;

        private string AccountId => accountIdTransport.AccountId;

        public PricingStrategyTemplateCreator(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public TPricingStrategy CreateTemplate(string intentId, string tableId)
        {
            var template = (ICreatePricingStrategyTemplate<TPricingStrategy>)Activator.CreateInstance<TPricingStrategy>();
            return template.CreateTemplate(AccountId, intentId, tableId);
        }

        public string GetPrettyName()
        {
            var template = (IHaveAPrettyNameAndTableType)Activator.CreateInstance<TPricingStrategy>();
            return template.GetPrettyName();
        }

        public string GetTableType()
        {
            var template = (IHaveAPrettyNameAndTableType)Activator.CreateInstance<TPricingStrategy>();
            return template.GetTableType();
        }
    }

    public class CreatePricingStrategyTableResponse<TR> where TR : IPricingStrategyTableRowResource
    {
        public CreatePricingStrategyTableResponse(PricingStrategyTableMetaResource response) => Response = response;
        public PricingStrategyTableMetaResource Response { get; set; }
    }

    public class CreatePricingStrategyTableRequest<T, TR, TCompiler>
        : IRequest<CreatePricingStrategyTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, new()
        where TR : IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
    {
        public const string Route = "create/{intentId}";

        public static string FormatRoute(string intentId)
        {
            return Route.Replace("{intentId}", intentId);
        }

        public CreatePricingStrategyTableRequest(string intentId)
        {
            IntentId = intentId;
        }

        public CreatePricingStrategyTableRequest()
        {
        }

        public string IntentId { get; set; }
    }
}