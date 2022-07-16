using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIsMultiOptionTypeHandler : IRequestHandler<GetIsMultiOptionTypeRequest, GetIsMultiOptionTypeResponse>
    {
        private readonly IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore;
        private readonly ILogger<GetIsMultiOptionTypeHandler> logger;
        private readonly IPricingStrategyTypeLister pricingStrategyTypeLister;
        private readonly IGuidFinder guidFinder;

        public GetIsMultiOptionTypeHandler(
            ILogger<GetIsMultiOptionTypeHandler> logger,
            IPricingStrategyTypeLister pricingStrategyTypeLister,
            IGuidFinder guidFinder,
            IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore)
        {
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.logger = logger;
            this.pricingStrategyTypeLister = pricingStrategyTypeLister;
            this.guidFinder = guidFinder;
        }

        public async Task<GetIsMultiOptionTypeResponse> Handle(GetIsMultiOptionTypeRequest request, CancellationToken cancellationToken)
        {
            foreach (var defaultNodeType in DefaultNodeTypeOptions.DefaultNodeTypeOptionsList)
            {
                if (request.NodeType.StartsWith(defaultNodeType.Value))
                {
                    return new GetIsMultiOptionTypeResponse(defaultNodeType.IsMultiOptionType);
                }
            }

            // node is a dynamic table node type
            // Comes in as e.g. SelectOneFlat-234234-324-2342-324
            foreach (var dynamicTableType in pricingStrategyTypeLister.ListPricingStrategies())
            {
                if (request.NodeType.StartsWith(dynamicTableType.GetTableType()))
                {
                    var tableId = guidFinder.FindFirstGuidSuffixOrNull(request.NodeType);
                    var table = await dynamicTableMetaStore.Get(tableId, s => s.TableId);
                    if (table != null)
                    {
                        var isMultiOption = table.ValuesAsPaths;
                        return new GetIsMultiOptionTypeResponse(isMultiOption);
                    }
                }
            }

            throw new Exception("NodeType not found.");
        }
    }

    public class GetIsMultiOptionTypeResponse
    {
        public GetIsMultiOptionTypeResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetIsMultiOptionTypeRequest : IRequest<GetIsMultiOptionTypeResponse>
    {
        public GetIsMultiOptionTypeRequest(string nodeType)
        {
            NodeType = nodeType;
        }

        public string NodeType { get; set; }
    }
}