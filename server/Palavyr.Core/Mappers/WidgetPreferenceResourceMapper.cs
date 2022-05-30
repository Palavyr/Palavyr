using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class WidgetPreferenceResourceMapper : IMapToNew<WidgetPreference, WidgetPreferenceResource>
    {
        public async Task<WidgetPreferenceResource> Map(WidgetPreference @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new WidgetPreferenceResource
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