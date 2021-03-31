using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class GetNodeTypeOptionsController : PalavyrBaseController
    {
        private ILogger<GetNodeTypeOptionsController> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IDynamicTableCompilerOrchestrator dynamicTableCompilerOrchestrator;

        public GetNodeTypeOptionsController(
            ILogger<GetNodeTypeOptionsController> logger,
            IConfigurationRepository configurationRepository,
            IDynamicTableCompilerOrchestrator dynamicTableCompilerOrchestrator
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
            this.dynamicTableCompilerOrchestrator = dynamicTableCompilerOrchestrator;
        }

        [HttpGet("configure-conversations/{areaId}/node-type-options")]
        public async Task<NodeTypeOption[]> Get([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var dynamicTableMetas = await configurationRepository.GetDynamicTableMetas(accountId, areaId);
            var dynamicTableData = await dynamicTableCompilerOrchestrator.CompileTablesToConfigurationNodes(dynamicTableMetas, accountId, areaId);
            var defaultNodeTypeOptions = DefaultNodeTypeOptions.DefaultNodeTypeOptionsList;

            var fullNodeTypeOptionsList = defaultNodeTypeOptions.AddAdditionalNodes(dynamicTableData);

            return fullNodeTypeOptionsList.ToArray();
        }
    }
}