using System.Linq;
using System.Net;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.API.ResponseTypes;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.controllers.widget
{
    [Authorize]
    [Route("api/widgetconfig/")]
    [ApiController]
    public class WidgetConfiguration : BaseController
    {
        private static ILogger<WidgetConfiguration> _logger;

        public WidgetConfiguration(
            ILogger<WidgetConfiguration> logger,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env
        ) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
        }

        [HttpGet("preferences")]
        public WidgetPreference GetWidgetPreferences([FromHeader] string accountId)
        {
            var prefs = DashContext.WidgetPreferences.SingleOrDefault(row => row.AccountId == accountId);
            return prefs;
        }


        [HttpPut("preferences")]
        public HttpStatusCode SaveWidgetPreferences([FromHeader] string accountId, [FromBody] WidgetPreference preferences)
        {
            var prefs = DashContext.WidgetPreferences.SingleOrDefault(row => row.AccountId == accountId);
            if (prefs == null) return HttpStatusCode.NotFound;
            if (!string.IsNullOrWhiteSpace(preferences.Title))
            {
                prefs.Title = preferences.Title;
            }

            if (!string.IsNullOrWhiteSpace(preferences.Subtitle))
            {
                prefs.Subtitle = preferences.Subtitle;
            }

            if (!string.IsNullOrWhiteSpace(preferences.Placeholder))
            {
                prefs.Placeholder = preferences.Placeholder;
            }

            if (!string.IsNullOrWhiteSpace(preferences.Header))
            {
                prefs.Header = preferences.Header;
            }

            if (!string.IsNullOrWhiteSpace(preferences.HeaderColor))
            {
                prefs.HeaderColor = preferences.HeaderColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.SelectListColor))
            {
                prefs.SelectListColor = preferences.SelectListColor;
            }

            if (!string.IsNullOrWhiteSpace(preferences.FontFamily))
            {
                prefs.FontFamily = preferences.FontFamily;
            }
            
            DashContext.SaveChanges();
            return HttpStatusCode.OK;

        }

        [HttpGet("demo/precheck")]
        public PreCheckResult RunDemoPreCheck([FromHeader] string accountId)
        {
            _logger.LogDebug("Collecting areas for DEMO pre-check...");
            var areas = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .ToList();

            _logger.LogDebug("Collected areas.... running DEMO pre-check");
            var result = PreCheckUtils.RunConversationsPreCheck(areas, _logger);
                
            _logger.LogDebug($"Pre-check run successful. Result: Isready -- {result.IsReady} and Incomplete areas: {result.IncompleteAreas.ToList()}");
            return result;
        }
    }
}