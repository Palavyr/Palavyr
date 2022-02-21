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
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<ModifyWidgetPreferencesHandler> logger;

        public ModifyWidgetPreferencesHandler(IConfigurationRepository configurationRepository, ILogger<ModifyWidgetPreferencesHandler> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        public async Task<ModifyWidgetPreferencesResponse> Handle(ModifyWidgetPreferencesRequest request, CancellationToken cancellationToken)
        {
            var prefs = await configurationRepository.GetWidgetPreferences();

            if (!string.IsNullOrWhiteSpace(request.SelectListColor))
            {
                prefs.SelectListColor = request.SelectListColor;
            }

            if (!string.IsNullOrWhiteSpace(request.HeaderColor))
            {
                prefs.HeaderColor = request.HeaderColor;
            }

            if (!string.IsNullOrWhiteSpace(request.FontFamily))
            {
                prefs.FontFamily = request.FontFamily;
            }

            if (!string.IsNullOrWhiteSpace(request.LandingHeader))
            {
                prefs.LandingHeader = request.LandingHeader;
            }

            if (!string.IsNullOrWhiteSpace(request.ChatHeader))
            {
                prefs.ChatHeader = request.ChatHeader;
            }

            if (!string.IsNullOrWhiteSpace(request.Placeholder))
            {
                prefs.Placeholder = request.Placeholder;
            }

            if (!string.IsNullOrWhiteSpace(request.ListFontColor))
            {
                prefs.ListFontColor = request.ListFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.HeaderFontColor))
            {
                prefs.HeaderFontColor = request.HeaderFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.OptionsHeaderColor))
            {
                prefs.OptionsHeaderColor = request.OptionsHeaderColor;
            }

            if (!string.IsNullOrWhiteSpace(request.OptionsHeaderFontColor))
            {
                prefs.OptionsHeaderFontColor = request.OptionsHeaderFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.ChatFontColor))
            {
                prefs.ChatFontColor = request.ChatFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.ButtonColor))
            {
                prefs.ButtonColor = request.ButtonColor;
            }

            if (!string.IsNullOrWhiteSpace(request.ButtonFontColor))
            {
                prefs.ButtonFontColor = request.ButtonFontColor;
            }

            if (!string.IsNullOrWhiteSpace(request.ChatBubbleColor))
            {
                prefs.ChatBubbleColor = request.ChatBubbleColor;
            }

            await configurationRepository.CommitChangesAsync();

            return new ModifyWidgetPreferencesResponse(prefs);
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