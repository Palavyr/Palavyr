using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;
using System.Linq;


namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    [ApiController]
    public class ModifyConversationNodeController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<ModifyConversationNodeController> logger;

        public ModifyConversationNodeController(
            DashContext dashContext,
            ILogger<ModifyConversationNodeController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpPut("configure-conversations/{areaId}/nodes/{nodeId}")]
        public async Task<IActionResult> Modify(
            [FromHeader] string accountId,
            [FromRoute] string nodeId,
            [FromRoute] string areaId,
            [FromBody] ConversationNode newNode)
        {
            var toRemove = dashContext.ConversationNodes.Where(row => row.NodeId == nodeId);
            dashContext.ConversationNodes.RemoveRange(toRemove);

            var area = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(p => p.ConversationNodes)
                .SingleOrDefaultAsync();

            var updatedConvo = area
                .ConversationNodes
                .Where(row => row.NodeId != nodeId)
                .ToList();
            
            var updatedNode = ConversationNode.CreateNew(
                newNode.NodeId,
                newNode.NodeType,
                newNode.Text,
                newNode.AreaIdentifier,
                newNode.NodeChildrenString,
                newNode.OptionPath,
                newNode.ValueOptions,
                accountId,
                newNode.IsRoot,
                newNode.IsCritical,
                newNode.IsMultiOptionType,
                newNode.IsTerminalType
            );

            updatedConvo.Add(updatedNode);
            area.ConversationNodes = updatedConvo;
            await dashContext.SaveChangesAsync();
            return NoContent();
        }
    }
}