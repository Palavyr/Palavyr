using System.Linq;
using System.Net;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.controllers.widget
{
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
            
            
            
            DashContext.SaveChanges();
            return HttpStatusCode.OK;

        }
    }
}