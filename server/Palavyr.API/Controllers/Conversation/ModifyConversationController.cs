using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    [ApiController]
    public class ModifyConversationController : ControllerBase
    {
        private ILogger<ModifyConversationController> logger;
        private DashContext dashContext;

        public ModifyConversationController(
            ILogger<ModifyConversationController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPut("configure-conversations/{areaId}")]
        public async Task<ConversationNode[]> Modify(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromBody] ConversationConfigurationUpdate update)
        {
            var nodesToDelete = dashContext
                .ConversationNodes
                .Where(row => update.IdsToDelete.Contains(row.NodeId));
            dashContext.ConversationNodes.RemoveRange(nodesToDelete);

            var area = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(p => p.ConversationNodes)
                .SingleOrDefaultAsync();

            var mappedTransactions = new List<ConversationNode>();
            foreach (var node in update.Transactions)
            {
                var mappedNode = ConversationNode.CreateNew(
                    node.NodeId,
                    node.NodeType,
                    node.Text,
                    node.AreaIdentifier,
                    node.NodeChildrenString,
                    node.OptionPath,
                    node.ValueOptions,
                    accountId,
                    node.IsRoot,
                    node.IsCritical,
                    node.IsMultiOptionType,
                    node.IsTerminalType
                );
                mappedTransactions.Add(mappedNode);
            }

            area.ConversationNodes.AddRange(mappedTransactions);
            await dashContext.SaveChangesAsync();

            var newNodes = dashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToArray();
            return newNodes;
        }
    }
}