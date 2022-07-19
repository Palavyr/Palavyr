using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIsTerminalTypeHandler : IRequestHandler<GetIsTerminalTypeRequest, GetIsTerminalTypeResponse>
    {
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore;
        private readonly IPricingStrategyTypeLister pricingStrategyTypeLister;
        string GUIDPattern = @"[{(]?\b[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}\b[)}]?";

        public GetIsTerminalTypeHandler(IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore, IPricingStrategyTypeLister pricingStrategyTypeLister)
        {
            this.pricingStrategyTableMetaStore = pricingStrategyTableMetaStore;
            this.pricingStrategyTypeLister = pricingStrategyTypeLister;
        }

        public async Task<GetIsTerminalTypeResponse> Handle(GetIsTerminalTypeRequest request, CancellationToken cancellationToken)
        {
            foreach (var defaultNodeType in DefaultNodeTypeOptions.DefaultNodeTypeOptionsList)
            {
                if (request.NodeType == defaultNodeType.Value)
                {
                    return new GetIsTerminalTypeResponse(defaultNodeType.IsTerminalType);
                }
            }

            // node is a pricing strategy table node type
            // Comes in as e.g. SelectOneFlat-234234-324-2342-324
            foreach (var pricingStrategyTableType in pricingStrategyTypeLister.ListPricingStrategies())
            {
                if (request.NodeType.StartsWith(pricingStrategyTableType.GetTableType()))
                {
                    var tableId = Regex.Match(request.NodeType, GUIDPattern, RegexOptions.IgnoreCase).Value;
                    var table = await pricingStrategyTableMetaStore.Get(tableId, s => s.TableId);
                    if (table != null)
                    {
                        return new GetIsTerminalTypeResponse(false);
                    }
                }
            }

            throw new Exception("DefaultNodeType not found.");
        }
    }

    public class GetIsTerminalTypeResponse
    {
        public GetIsTerminalTypeResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetIsTerminalTypeRequest : IRequest<GetIsTerminalTypeResponse>
    {
        public string NodeType { get; set; }

        public GetIsTerminalTypeRequest(string nodeType)
        {
            NodeType = nodeType;
        }
    }
}