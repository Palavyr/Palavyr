using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIsTerminalTypeHandler : IRequestHandler<GetIsTerminalTypeRequest, GetIsTerminalTypeResponse>
    {
        string GUIDPattern = @"[{(]?\b[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}\b[)}]?";
        private DashContext dashContext;

        public GetIsTerminalTypeHandler(DashContext dashContext)
        {
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
            foreach (var dynamicTableType in DynamicTableTypes.GetDynamicTableTypes())
            {
                if (request.NodeType.StartsWith(dynamicTableType.TableType))
                {
                    var tableId = Regex.Match(request.NodeType, GUIDPattern, RegexOptions.IgnoreCase).Value;
                    var table = await dashContext
                        .DynamicTableMetas
                        .SingleOrDefaultAsync(row => row.TableId == tableId);
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