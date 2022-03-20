using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetNodeTypeOptionsHandler : IRequestHandler<GetNodeTypeOptionsRequest, GetNodeTypeOptionsResponse>
    {
        private readonly ILogger<GetNodeTypeOptionsHandler> logger;
        private readonly IDynamicTableCompilerOrchestrator dynamicTableCompilerOrchestrator;
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;

        public GetNodeTypeOptionsHandler(
            ILogger<GetNodeTypeOptionsHandler> logger,
            IDynamicTableCompilerOrchestrator dynamicTableCompilerOrchestrator,
            IEntityStore<DynamicTableMeta> dynamicTableMetaStore)
        {
            this.logger = logger;
            this.dynamicTableCompilerOrchestrator = dynamicTableCompilerOrchestrator;
            this.dynamicTableMetaStore = dynamicTableMetaStore;
        }

        public async Task<GetNodeTypeOptionsResponse> Handle(GetNodeTypeOptionsRequest request, CancellationToken cancellationToken)
        {
            var dynamicTableMetas = await dynamicTableMetaStore.GetMany(request.IntentId, s => s.AreaIdentifier);
            var dynamicTableData = await dynamicTableCompilerOrchestrator.CompileTablesToConfigurationNodes(dynamicTableMetas, request.IntentId);
            var defaultNodeTypeOptions = DefaultNodeTypeOptions.DefaultNodeTypeOptionsList;

            var fullNodeTypeOptionsList = defaultNodeTypeOptions.AddAdditionalNodes(dynamicTableData);

            return new GetNodeTypeOptionsResponse(fullNodeTypeOptionsList.ToArray());
        }
    }

    public class GetNodeTypeOptionsResponse
    {
        public GetNodeTypeOptionsResponse(NodeTypeOption[] response) => Response = response;
        public NodeTypeOption[] Response { get; set; }
    }

    public class GetNodeTypeOptionsRequest : IRequest<GetNodeTypeOptionsResponse>
    {
        public GetNodeTypeOptionsRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}