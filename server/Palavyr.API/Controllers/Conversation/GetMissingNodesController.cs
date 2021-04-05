using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Conversation
{
    public class RequiredDetails
    {
        public string Type { get; set; }
        public string PrettyName { get; set; }

        public static RequiredDetails Create(string type, string prettyName)
        {
            return new RequiredDetails()
            {
                Type = type,
                PrettyName = prettyName
            };
        }
    }


    public class GetMissingNodesController : PalavyrBaseController
    {
        private ILogger<GetMissingNodesController> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly RequiredNodeCalculator requiredNodeCalculator;
        private readonly MissingNodeCalculator missingNodeCalculator;

        public GetMissingNodesController(
            ILogger<GetMissingNodesController> logger,
            IConfigurationRepository configurationRepository,
            RequiredNodeCalculator requiredNodeCalculator,
            MissingNodeCalculator missingNodeCalculator
        )
        {
            this.configurationRepository = configurationRepository;
            this.requiredNodeCalculator = requiredNodeCalculator;
            this.missingNodeCalculator = missingNodeCalculator;
            this.logger = logger;
        }

        [HttpPost("configure-conversations/{areaId}/missing-nodes")]
        public async Task<IEnumerable<string>> Get([FromHeader] string accountId, string areaId, [FromBody] ConversationNodeDto currentNodes)
        {
            var area = await configurationRepository.GetAreaComplete(accountId, areaId);
            
            var dynamicTableMetas = area.DynamicTableMetas;
            var staticTableMetas = area.StaticTablesMetas;

            var requiredDynamicNodeTypes = await requiredNodeCalculator.FindRequiredNodes(area);
            var allMissingNodeTypeNames = missingNodeCalculator.CalculateMissingNodes(requiredDynamicNodeTypes.ToArray(), currentNodes.Transactions, dynamicTableMetas, staticTableMetas);
            return allMissingNodeTypeNames;
        }
    }
}