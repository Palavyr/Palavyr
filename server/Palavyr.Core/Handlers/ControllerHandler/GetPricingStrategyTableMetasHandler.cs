using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.Units;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetPricingStrategyTableMetasHandler : IRequestHandler<GetPricingStrategyTableMetasRequest, GetPricingStrategyTableMetasResponse>
    {
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore;
        private readonly ILogger<GetPricingStrategyTableMetasHandler> logger;
        private readonly IUnitRetriever unitRetriever;

        public GetPricingStrategyTableMetasHandler(
            IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore,
            ILogger<GetPricingStrategyTableMetasHandler> logger,
            IUnitRetriever unitRetriever)
        {
            this.pricingStrategyTableMetaStore = pricingStrategyTableMetaStore;
            this.logger = logger;
            this.unitRetriever = unitRetriever;
        }

        public async Task<GetPricingStrategyTableMetasResponse> Handle(GetPricingStrategyTableMetasRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Retrieve Pricing Strategy Table Metas");
            var tableTypes = await pricingStrategyTableMetaStore.GetMany(request.IntentId, s => s.IntentId);

            
            // TODO: Write a mapper for this
            var tableResources = tableTypes.Select(
                    x =>
                    {
                        var unitDefinition = unitRetriever.GetUnitDefinitionById(x.UnitIdEnum);
                        return new PricingStrategyTableMetaResource
                        {
                            Id = x.Id.Value,
                            TableTag = x.TableTag,
                            TableType = x.TableType,
                            TableId = x.TableId,
                            IntentId = x.IntentId,
                            ValuesAsPaths = x.ValuesAsPaths,
                            PrettyName = x.PrettyName,
                            UnitPrettyName = unitDefinition.UnitPrettyName,
                            UnitGroup = unitDefinition.UnitGroup,
                            UnitIdEnum = unitDefinition.UnitIdEnum
                        };
                    })
                .ToArray();

            return new GetPricingStrategyTableMetasResponse(tableResources);
        }
    }


    public class GetPricingStrategyTableMetasResponse
    {
        public GetPricingStrategyTableMetasResponse(PricingStrategyTableMetaResource[] response) => Response = response;
        public PricingStrategyTableMetaResource[] Response { get; set; }
    }

    public class GetPricingStrategyTableMetasRequest : IRequest<GetPricingStrategyTableMetasResponse>
    {
        public GetPricingStrategyTableMetasRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}