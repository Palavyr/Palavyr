using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.responseTypes;
using Palavyr.API.ResponseTypes;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.WidgetScheme)]
    [Route("api/widget")] 
    [ApiController]
    public class WidgetAccess : BaseController
    {
        private static ILogger<WidgetAccess> _logger;

        public WidgetAccess(
            ILogger<WidgetAccess> logger,
            AccountsContext accountsContext,
            ConvoContext convoContext, 
            DashContext dashContext, 
            IWebHostEnvironment env) : base(accountsContext,
            convoContext, dashContext, env)
        {
            _logger = logger;
        }
        
        [HttpGet("areas")]
        public List<Area> FetchAreas([FromHeader] string accountId)
        {
            return DashContext.Areas.Where(row => row.AccountId == accountId).ToList();
        }

        [HttpGet("groups")]
        public List<GroupMap> FetchGroups([FromHeader] string accountId)
        {
            return DashContext.Groups.Where(row => row.AccountId == accountId).ToList();
        }

        [HttpGet("{areaId}/create")]
        public NewConversation CreateConvo([FromHeader] string accountId, string areaId)
        {
            _logger.LogDebug("Fetching Preferences...");
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

        [HttpGet("preferences")]
        public WidgetPreference FetchPreferences([FromHeader] string accountId)
        {
            var prefs = DashContext.WidgetPreferences.SingleOrDefault(row => row.AccountId == accountId);
            return prefs;
        }
        
        /// <summary>
        /// Used by the widget
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet("precheck")]
        public PreCheckResult RunWidgetPreCheck([FromHeader] string accountId)
        {
            _logger.LogDebug("Running live widget pre-check...");

            _logger.LogDebug("Checking if account ID exists...");
            if (accountId == null)
            {
                return PreCheckResult.CreateApiKeyResult(false);
            }
            _logger.LogDebug($"Using AccountId: {accountId}");
            var areas = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .ToList();
            
            _logger.LogDebug("Running live widget conversations pre-check...");
            var result = PreCheckUtils.RunConversationsPreCheck(areas, _logger);
            _logger.LogDebug($"Pre-check run successful. Result: Isready:{result.IsReady} and incomplete areas: {result.IncompleteAreas.ToList()}");
            return result;
        }

        [HttpPost("complete")]
        public HttpStatusCode SetCompletedConversation([FromHeader] string accountId, CompleteConversation completeConvo)
        {
            _logger.LogDebug("Adding completed conversation to the database.");
            
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
                
            // TODO: Add validation on the phone number and the name perhaps
            ConvoContext.CompletedConversations.Add(completedConversation);
            ConvoContext.SaveChanges();
            return HttpStatusCode.OK;
        }
    }
}