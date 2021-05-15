using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.API.controllers.Conversation
{
    public class GetIsMultiOptionTypeController : PalavyrBaseController
    {
        private ILogger<GetIsMultiOptionTypeController> logger;
        private readonly GuidFinder guidFinder;
        private DashContext dashContext;

        public GetIsMultiOptionTypeController(
            ILogger<GetIsMultiOptionTypeController> logger,
            GuidFinder guidFinder,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.guidFinder = guidFinder;
            this.dashContext = dashContext;
        }

        [HttpGet("configure-conversations/check-multi-option/{nodeType}")]
        public async Task<bool> Get(string nodeType)
        {
            foreach (var defaultNodeType in DefaultNodeTypeOptions.DefaultNodeTypeOptionsList)
            {
                if (nodeType.StartsWith(defaultNodeType.Value))
                {
                    return defaultNodeType.IsMultiOptionType;
                }
            }

            // node is a dynamic table node type
            // Comes in as e.g. SelectOneFlat-234234-324-2342-324
            foreach (var dynamicTableType in DynamicTableTypes.GetDynamicTableTypes())
            {
                if (nodeType.StartsWith(dynamicTableType.TableType))
                {
                    var tableId = guidFinder.FindGuid(nodeType);
                    var table = await dashContext
                        .DynamicTableMetas
                        .SingleOrDefaultAsync(row => row.TableId == tableId);
                    if (table != null)
                    {
                        var isMultiOption = table.ValuesAsPaths;
                        return isMultiOption;
                    }
                }
            }

            throw new Exception("NodeType not found.");
        }
    }
}