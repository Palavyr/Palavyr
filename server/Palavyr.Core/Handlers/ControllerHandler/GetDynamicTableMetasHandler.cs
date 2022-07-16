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
    public class GetDynamicTableMetasHandler : IRequestHandler<GetDynamicTableMetasRequest, GetDynamicTableMetasResponse>
    {
        private readonly IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore;
        private readonly ILogger<GetDynamicTableMetasHandler> logger;
        private readonly IUnitRetriever unitRetriever;

        public GetDynamicTableMetasHandler(
            IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore,
            ILogger<GetDynamicTableMetasHandler> logger,
            IUnitRetriever unitRetriever)
        {
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.logger = logger;
            this.unitRetriever = unitRetriever;
        }

        public async Task<GetDynamicTableMetasResponse> Handle(GetDynamicTableMetasRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Retrieve Dynamic Table Metas");
            var tableTypes = await dynamicTableMetaStore.GetMany(request.IntentId, s => s.AreaIdentifier);

            
            // TODO: Write a mapper for this
            var tableResources = tableTypes.Select(
                    x =>
                    {
                        var unitDefinition = unitRetriever.GetUnitDefinitionById(x.UnitId);
                        return new PricingStrategyTableMetaResource
                        {
                            Id = x.Id,
                            TableTag = x.TableTag,
                            TableType = x.TableType,
                            TableId = x.TableId,
                            AreaIdentifier = x.AreaIdentifier,
                            ValuesAsPaths = x.ValuesAsPaths,
                            PrettyName = x.PrettyName,
                            UnitPrettyName = unitDefinition.UnitPrettyName,
                            UnitGroup = unitDefinition.UnitGroup,
                            UnitId = unitDefinition.UnitId
                        };
                    })
                .ToArray();

            return new GetDynamicTableMetasResponse(tableResources);
        }
    }


    public class GetDynamicTableMetasResponse
    {
        public GetDynamicTableMetasResponse(PricingStrategyTableMetaResource[] response) => Response = response;
        public PricingStrategyTableMetaResource[] Response { get; set; }
    }

    public class GetDynamicTableMetasRequest : IRequest<GetDynamicTableMetasResponse>
    {
        public GetDynamicTableMetasRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}