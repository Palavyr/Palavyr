using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Constant;

namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    public class GetIsTerminalTypeController : ControllerBase
    {
        private ILogger<GetIsTerminalTypeController> logger;
        string GUIDPattern = @"[{(]?\b[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}\b[)}]?";
        private DashContext dashContext;

        public GetIsTerminalTypeController(
            ILogger<GetIsTerminalTypeController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpGet("configure-conversations/check-terminal/{nodeType}")]
        public async Task<bool> Get(string nodeType)
        {
            foreach (var defaultNodeType in DefaultNodeTypeOptions.DefaultNodeTypeOptionsList)
            {
                if (nodeType == defaultNodeType.Value)
                {
                    return defaultNodeType.IsTerminalType;
                }
            }
            
            // node is a dynamic table node type
            // Comes in as e.g. SelectOneFlat-234234-324-2342-324
            foreach (var dynamicTableType in DynamicTableTypes.GetDynamicTableTypes())
            {
                if (nodeType.StartsWith(dynamicTableType.TableType))
                {
                    var tableId = Regex.Match(nodeType, GUIDPattern, RegexOptions.IgnoreCase).Value;
                    var table = await dashContext
                        .DynamicTableMetas
                        .SingleOrDefaultAsync(row => row.TableId == tableId);
                    if (table != null)
                    {
                        return false;
                    }
                }
            }
                
            throw new Exception("DefaultNodeType not found.");
        }
    }
}