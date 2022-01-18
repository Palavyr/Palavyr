using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.Core.Handlers
{
    public class GetNodeTypeOptionsHandler : IRequestHandler<GetNodeTypeOptionsRequest, GetNodeTypeOptionsResponse>
    {
        private readonly ILogger<GetNodeTypeOptionsHandler> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IDynamicTableCompilerOrchestrator dynamicTableCompilerOrchestrator;

        public GetNodeTypeOptionsHandler(
            ILogger<GetNodeTypeOptionsHandler> logger,
            IConfigurationRepository configurationRepository,
            IDynamicTableCompilerOrchestrator dynamicTableCompilerOrchestrator)
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
            this.dynamicTableCompilerOrchestrator = dynamicTableCompilerOrchestrator;
        }

        public async Task<GetNodeTypeOptionsResponse> Handle(GetNodeTypeOptionsRequest request, CancellationToken cancellationToken)
        {
            var dynamicTableMetas = await configurationRepository.GetDynamicTableMetas(request.IntentId);
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