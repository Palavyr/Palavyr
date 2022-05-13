using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIsTerminalTypeHandler : IRequestHandler<GetIsTerminalTypeRequest, GetIsTerminalTypeResponse>
    {
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;
        string GUIDPattern = @"[{(]?\b[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}\b[)}]?";

        public GetIsTerminalTypeHandler(IEntityStore<DynamicTableMeta> dynamicTableMetaStore)
        {
            this.dynamicTableMetaStore = dynamicTableMetaStore;
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

            // node is a dynamic table node type
            // Comes in as e.g. SelectOneFlat-234234-324-2342-324
            foreach (var dynamicTableType in PricingStrategyTableTypes.GetDynamicTableTypes())
            {
                if (request.NodeType.StartsWith(dynamicTableType.TableType))
                {
                    var tableId = Regex.Match(request.NodeType, GUIDPattern, RegexOptions.IgnoreCase).Value;
                    var table = await dynamicTableMetaStore.Get(tableId, s => s.TableId);
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