using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.responseTypes;
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

        private string? GetAccountId(string apiKey)
        {
            var account = AccountContext.Accounts.SingleOrDefault(row => row.ApiKey == apiKey);
            var accountId = account?.AccountId;
            return accountId;
        }

        [HttpGet("{apiKey}/areas")]
        public List<Area> FetchAreas(string apiKey)
        {
            _logger.LogDebug("Fetching areas");
            var accountId = GetAccountId(apiKey);
            if (accountId == null) throw new Exception(); // this should never be null!
            return DashContext.Areas.Where(row => row.AccountId == accountId).ToList();
        }

        [HttpGet("{apiKey}/groups")]
        public List<GroupMap> GetGroups(string apiKey)
        {
            var accountId = GetAccountId(apiKey);
            return DashContext.Groups.Where(row => row.AccountId == accountId).ToList();
        }

        [HttpGet("{apiKey}/{areaId}/create")]
        public NewConversation CreateConversation(string apiKey, string areaId)
        {
            _logger.LogDebug("Fetching Preferences...");
            var accountId = GetAccountId(apiKey);
            var widgetPreference = DashContext.WidgetPreferences.Single(row => row.AccountId == accountId);
            
            _logger.LogDebug("Fetching nodes...");
            var incompleteNodeList = DashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToList();
            var convoNodes = EndingSequence.AttachEndingSequenceToNodeList(incompleteNodeList, areaId, accountId);
            
            _logger.LogDebug("Creating new conversation for user with apikey: {apiKey}");
            var newConvo = NewConversation.CreateNew(widgetPreference, convoNodes);

            return newConvo;
        }
        
        
        /// <summary>
        /// Used by the widget
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet("{apiKey}/precheck")]
        public PreCheckResult RunWidgetPreCheck(string apiKey)
        {
            _logger.LogDebug("Running live widget pre-check...");

            _logger.LogDebug("Checking if account ID exists...");
            var accountId = GetAccountId(apiKey);
            if (accountId == null)
            {
                return PreCheckResult.CreateApiKeyResult(false);
            }
            _logger.LogDebug($"Using AccountId: {accountId}");
            var areas = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .ToList();
            
            _logger.LogDebug("Running live widget conversations pre-check...");
            PreCheckResult result = PreCheckUtils.RunConversationsPreCheck(areas, _logger);
            _logger.LogDebug($"Pre-check run successful. Result: Isready:{result.IsReady} and incomplete areas: {result.IncompleteAreas.ToList()}");
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
            _logger.LogDebug("Collecting areas for DEMO pre-check...");
            var areas = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .ToList();

            _logger.LogDebug("Collected areas.... running DEMO pre-check");
            PreCheckResult result = PreCheckUtils.RunConversationsPreCheck(areas, _logger);
            
            _logger.LogDebug($"Pre-check run successful. Result: Isready -- {result.IsReady} and Incomplete areas: {result.IncompleteAreas.ToList()}");
            return result;
        }

        [HttpPost("{apiKey}/complete")]
        public HttpStatusCode SetCompletedConversation(string apiKey, CompleteConversation completeConvo)
        {
            _logger.LogDebug("Adding completed conversation to the database.");

            var accountId = GetAccountId(apiKey);

            var area = DashContext.Areas.Single(row =>
                row.AccountId == accountId && row.AreaIdentifier == completeConvo.AreaIdentifier);
            var areaName = area.AreaName;
            var emailTemplateUsed = area.EmailTemplate;
            
            var conversationId = completeConvo.ConversationId;
            var email = completeConvo.Email;
            var name = completeConvo.Name;
            var phone = completeConvo.PhoneNumber;

            var completedConversation = CompleteConversation.BindReceiverToSchemaType(conversationId, accountId,
                areaName, emailTemplateUsed, name, email, phone);
                
            // TODO: Add valiation on the phone number and the name perhaps
            ConvoContext.CompletedConversations.Add(completedConversation);
            ConvoContext.SaveChanges();
            return HttpStatusCode.OK;
        }
        
    }

    
}