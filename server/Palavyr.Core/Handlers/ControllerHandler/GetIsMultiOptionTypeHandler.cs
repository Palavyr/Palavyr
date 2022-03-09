using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIsMultiOptionTypeHandler : IRequestHandler<GetIsMultiOptionTypeRequest, GetIsMultiOptionTypeResponse>
    {
        private readonly ILogger<GetIsMultiOptionTypeHandler> logger;
        private readonly GuidFinder guidFinder;
        private readonly DashContext dashContext;

        public GetIsMultiOptionTypeHandler(
            ILogger<GetIsMultiOptionTypeHandler> logger,
            GuidFinder guidFinder,
            DashContext dashContext)
        {
            this.logger = logger;
            this.guidFinder = guidFinder;
            this.dashContext = dashContext;
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
            foreach (var dynamicTableType in DynamicTableTypes.GetDynamicTableTypes())
            {
                if (request.NodeType.StartsWith(dynamicTableType.TableType))
                {
                    var tableId = guidFinder.FindFirstGuidSuffixOrNull(request.NodeType);
                    var table = await dashContext
                        .DynamicTableMetas
                        .SingleOrDefaultAsync(row => row.TableId == tableId);
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