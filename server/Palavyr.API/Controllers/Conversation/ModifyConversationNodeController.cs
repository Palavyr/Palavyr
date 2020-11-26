using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

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
        
        [HttpPut("configure-conversations/nodes/{nodeId}")]
        public async Task<IActionResult> Modify(
            [FromHeader] string accountId, 
            [FromRoute] string nodeId, 
            [FromBody] ConversationNode newNode)
        {
            logger.LogDebug($"Updating Conversation node: {nodeId}");
            try
            {
                dashContext.ConversationNodes.Remove(
                    await dashContext
                        .ConversationNodes
                        .SingleOrDefaultAsync(row => row.NodeId == nodeId));

                var mappedNode = ConversationNode.CreateNew(
                    newNode.NodeId,
                    newNode.NodeType,
                    newNode.Text,
                    newNode.AreaIdentifier,
                    newNode.NodeChildrenString,
                    newNode.OptionPath,
                    newNode.ValueOptions,
                    accountId,
                    newNode.IsRoot,
                    newNode.IsCritical
                );

                dashContext.ConversationNodes.Add(mappedNode);
                await dashContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                logger.LogDebug("Could not update:" + message);
                return BadRequest();
            }
            return NoContent();
        }
        
    }
}