using System;
using System.Collections.Generic;
using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.API.receiverTypes;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.controllers.Conversation
{
    [Route("api/convos/")]
    [ApiController]
    public class ConversationController : BaseController
    {
        private static ILogger<PreviewController> _logger;

        public ConversationController(
            ILogger<PreviewController> logger,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env
        ) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;

        }

        [HttpGet]
        public IQueryable<ConversationNode> GetAllConvos([FromHeader] string accountId)
        {
            return DashContext.ConversationNodes.Where(row => row.AccountId == accountId);
        }


        [HttpGet("{areaId}")]
        public IQueryable<ConversationNode> GetConversation([FromHeader] string accountId, string areaId)
        {
            var result = DashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId);
            return result;
        }

        [HttpPut("update/{areaId}")]
        public OkResult UpdateConversation([FromHeader] string accountId, string areaId,
            [FromBody] List<ConversationNode> conversation)
        {
            var rowsToRemove = DashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId).ToArray();
            DashContext.ConversationNodes.RemoveRange(rowsToRemove);

            var mappedConversation = new List<ConversationNode>();
            foreach (var node in conversation)
            {
                var mappedNode = ConversationNode.CreateNew(
                    node.NodeId,
                    node.NodeType,
                    node.Text,
                    node.AreaIdentifier,
                    node.NodeChildrenString,
                    node.ValueOptions,
                    node.AccountId,
                    node.IsRoot,
                    node.IsCritical
                );
                mappedConversation.Add(mappedNode);
                // node.Id = null;
            }
            DashContext.ConversationNodes.AddRange(mappedConversation);
            DashContext.SaveChanges();
            return new OkResult();
        }

        [HttpGet("nodes/{nodeId}")]
        public ConversationNode GetConversationNode(string nodeId)
        {
            // node Ids are globally unique - don't need account Id Filter
            var result = DashContext.ConversationNodes.Single(row => row.NodeId == nodeId);
            return result;
        }

        [HttpPut("nodes/{nodeId}")]
        public OkResult UpdateConversationNode([FromHeader] string accountId, string nodeId, [FromBody] ConversationNode newNode)
        {
            _logger.LogInformation("------ UPDATING NODE ---------");
            try
            {


                DashContext.ConversationNodes.Remove(DashContext.ConversationNodes.Single(row => row.NodeId == nodeId));

                var mappedNode = ConversationNode.CreateNew(
                    newNode.NodeId,
                    newNode.NodeType,
                    newNode.Text,
                    newNode.AreaIdentifier,
                    newNode.NodeChildrenString,
                    newNode.ValueOptions,
                    accountId,
                    newNode.IsRoot,
                    newNode.IsCritical
                );

                DashContext.ConversationNodes.Add(mappedNode);
                DashContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                _logger.LogInformation("COULD NOT UPDATE BECAUSE:" + message);
            }

            return new OkResult();
        }

        [HttpPost("{areaId}")]
        public List<ConversationNode> PostConversationNodes([FromHeader] string accountId, string areaId,
            [FromBody] ConversationConfigurationUpdate update)
        {
            var nodesToDelete = DashContext.ConversationNodes.Where(row => update.IdsToDelete.Contains(row.NodeId));
            DashContext.ConversationNodes.RemoveRange(nodesToDelete);
            DashContext.SaveChanges();

            var area = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(p => p.ConversationNodes)
                .Single();

            var mappedTransactions = new List<ConversationNode>();
            foreach (var node in update.Transactions)
            {
                var mappedNode = ConversationNode.CreateNew(
                    node.NodeId,
                    node.NodeType,
                    node.Text,
                    node.AreaIdentifier,
                    node.NodeChildrenString,
                    node.ValueOptions,
                    accountId,
                    node.IsRoot,
                    node.IsCritical
                );
                mappedTransactions.Add(mappedNode);
            }

            area.ConversationNodes.AddRange(mappedTransactions);
            DashContext.SaveChanges();

            var newNodes = DashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToList();
            return newNodes;
        }
    }
}