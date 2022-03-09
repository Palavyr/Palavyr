using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyWidgetPreferencesHandler : IRequestHandler<ModifyWidgetPreferencesRequest, ModifyWidgetPreferencesResponse>
    {
        private readonly IConfigurationEntityStore<WidgetPreference> widgetPreferenceStore;
        private readonly ILogger<ModifyWidgetPreferencesHandler> logger;

        public ModifyWidgetPreferencesHandler(IConfigurationEntityStore<WidgetPreference> widgetPreferenceStore, ILogger<ModifyWidgetPreferencesHandler> logger)
        {
            this.widgetPreferenceStore = widgetPreferenceStore;
            this.logger = logger;
        }

        public async Task<ModifyWidgetPreferencesResponse> Handle(ModifyWidgetPreferencesRequest request, CancellationToken cancellationToken)
        {
            var widgetPreferences = await widgetPreferenceStore.Get(widgetPreferenceStore.AccountId, s => s.AccountId);

            if (!string.IsNullOrWhiteSpace(request.SelectListColor))
            {
                widgetPreferences.SelectListColor = request.SelectListColor;
            }

            if (!string.IsNullOrWhiteSpace(request.HeaderColor))
            {
                widgetPreferences.HeaderColor = request.HeaderColor;
            }

            if (!string.IsNullOrWhiteSpace(request.FontFamily))
            {
                widgetPreferences.FontFamily = request.FontFamily;
            }

            if (!string.IsNullOrWhiteSpace(request.LandingHeader))
            {
                widgetPreferences.LandingHeader = request.LandingHeader;
            }

            if (!string.IsNullOrWhiteSpace(request.ChatHeader))
            {
                widgetPreferences.ChatHeader = request.ChatHeader;
            }

            if (!string.IsNullOrWhiteSpace(request.Placeholder))
            {
                widgetPreferences.Placeholder = request.Placeholder;
            }

            if (!string.IsNullOrWhiteSpace(request.ListFontColor))
            {
                widgetPreferences.ListFontColor = request.ListFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.HeaderFontColor))
            {
                widgetPreferences.HeaderFontColor = request.HeaderFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.OptionsHeaderColor))
            {
                widgetPreferences.OptionsHeaderColor = request.OptionsHeaderColor;
            }

            if (!string.IsNullOrWhiteSpace(request.OptionsHeaderFontColor))
            {
                widgetPreferences.OptionsHeaderFontColor = request.OptionsHeaderFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.ChatFontColor))
            {
                widgetPreferences.ChatFontColor = request.ChatFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.ButtonColor))
            {
                widgetPreferences.ButtonColor = request.ButtonColor;
            }

            if (!string.IsNullOrWhiteSpace(request.ButtonFontColor))
            {
                widgetPreferences.ButtonFontColor = request.ButtonFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.ChatBubbleColor))
            {
                widgetPreferences.ChatBubbleColor = request.ChatBubbleColor;
            }

            return new ModifyWidgetPreferencesResponse(widgetPreferences);
        }
    }

    public class ModifyWidgetPreferencesResponse
    {
        public ModifyWidgetPreferencesResponse(WidgetPreference response) => Response = response;
        public WidgetPreference Response { get; set; }
    }

    public class ModifyWidgetPreferencesRequest : WidgetPreference, IRequest<ModifyWidgetPreferencesResponse>
    {
    }
}