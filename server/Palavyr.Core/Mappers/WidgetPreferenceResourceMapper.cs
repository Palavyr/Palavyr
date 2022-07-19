using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class WidgetPreferenceResourceMapper : IMapToNew<WidgetPreference, WidgetPreferencesResource>
    {
        public async Task<WidgetPreferencesResource> Map(WidgetPreference @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new WidgetPreferencesResource
            {
                HeaderFontColor = @from.HeaderFontColor,
                ListFontColor = @from.ListFontColor,
                SelectListColor = @from.SelectListColor,
                HeaderColor = @from.HeaderColor,
                FontFamily = @from.FontFamily,
                LandingHeader = @from.LandingHeader,
                ChatHeader = @from.ChatHeader,
                Placeholder = @from.Placeholder,
                AccountId = @from.AccountId,
                WidgetState = @from.WidgetState,
                OptionsHeaderColor = @from.OptionsHeaderColor,
                OptionsHeaderFontColor = @from.OptionsHeaderFontColor,
                ChatFontColor = @from.ChatFontColor,
                ChatBubbleColor = @from.ChatBubbleColor,
                ButtonColor = @from.ButtonColor,
                ButtonFontColor = @from.ButtonFontColor,
            };
        }
    }
}