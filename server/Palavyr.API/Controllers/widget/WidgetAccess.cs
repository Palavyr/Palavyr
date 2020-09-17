using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Cors;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.API.ResponseTypes;
using Server.Domain;

namespace Palavyr.API.Controllers
{
    // [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    [Route("api/widget")]
    [ApiController]
    public class WidgetAccess : BaseController
    {
        public WidgetAccess(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }

        private string GetAccountId(string apiKey)
        {
            var account = AccountContext.Accounts.Single(row => row.ApiKey == apiKey);
            return account.AccountId;
        }

        [HttpGet("{apiKey}/areas")]
        public List<Area> FetchAreas(string apiKey)
        {
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
            var accountId = GetAccountId(apiKey);

            var areas = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .ToList();
            return PreCheckUtils.RunConversationsPreCheck(areas);
        }
        
        /// <summary>
        /// Used by the dashboard
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("demo/precheck")]
        public PreCheckResult RunDemoPreCheck([FromHeader] string accountId)
        {
            var areas = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .ToList();
            return PreCheckUtils.RunConversationsPreCheck(areas);
        }
        
    }
}