using System;
using System.Collections.Generic;
using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.ResponseTypes;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    [Route("api/widget")]
    [ApiController]
    public class WidgetAccess : BaseController
    {
        private static ILogger<WidgetAccess> _logger;

        public WidgetAccess(
            ILogger<WidgetAccess> logger,
            AccountsContext accountContext,
            ConvoContext convoContext, 
            DashContext dashContext, 
            IWebHostEnvironment env) : base(accountContext,
            convoContext, dashContext, env)
        {
            _logger = logger;
        }

        private string GetAccountId(string apiKey)
        {
            var account = AccountContext.Accounts.Single(row => row.ApiKey == apiKey);
            return account.AccountId;
        }

        [HttpGet("{apiKey}/areas")]
        public List<Area> FetchAreas(string apiKey)
        {
            _logger.LogDebug("Fetching areas");
            var accountId = GetAccountId(apiKey);
            return DashContext.Areas.Where(row => row.AccountId == accountId).ToList();
        }

        [HttpGet("{apiKey}/groups")]
        public List<GroupMap> GetGroups(string apiKey)
        {
            var accountId = GetAccountId(apiKey);
            return DashContext.Groups.Where(row => row.AccountId == accountId).ToList();
        }

        [HttpGet("{apiKey}/{areaId}/nodes")]
        public List<ConversationNode> FetchNodes(string apiKey, string areaId)
        {
            _logger.LogDebug("Fetching nodes...");
            var accountId = GetAccountId(apiKey);
            var incompleteNodeList = DashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToList();
            var completeNodeList = EndingSequence.AttachEndingSequenceToNodeList(incompleteNodeList, areaId, accountId);
            return completeNodeList;
        }

        [HttpGet("{apiKey}/preferences")]
        public WidgetPreference ShouldUseGroups(string apiKey)
        {
            _logger.LogDebug("Fetching Preferences...");
            var accountId = GetAccountId(apiKey);
            var prefs = DashContext.WidgetPreferences.Single(row => row.AccountId == accountId);
            return prefs;
        }

        /// <summary>
        /// Used by the widget
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet("{apiKey}/precheck")]
        public PreCheckResult RunWidgetPreCheck(string apiKey)
        {
            _logger.LogDebug("Running Widget Precheck....");
            var accountId = GetAccountId(apiKey);
            _logger.LogDebug($"Using AccountId: {accountId}");
            var areas = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .ToList();
            _logger.LogDebug("Collected areas.... running precheck");
            PreCheckResult result = PreCheckUtils.RunConversationsPreCheck(areas);
            _logger.LogDebug($"Prechuck run successful. Result: Isready -- {result.IsReady} and Incomplete areas: {result.IncompleteAreas.ToList()}");
            return result;
        }

        /// <summary>
        /// Used by the dashboard
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("demo/precheck")]
        public PreCheckResult RunDemoPreCheck([FromHeader] string accountId)
        {
            _logger.LogDebug("Collecting areas for precheck...");
            var areas = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .ToList();

            _logger.LogDebug("Collected areas.... running precheck");
            PreCheckResult result = PreCheckUtils.RunConversationsPreCheck(areas);
            _logger.LogDebug($"Precheck run successful. Result: Isready -- {result.IsReady} and Incomplete areas: {result.IncompleteAreas.ToList()}");
            return result;
        }
    }
}