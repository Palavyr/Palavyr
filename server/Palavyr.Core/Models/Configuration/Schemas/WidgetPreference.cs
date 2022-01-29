using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class WidgetPreference
    {
        [Key]
        public int? Id { get; set; }

        public string Placeholder { get; set; }

        public string AccountId { get; set; }

        public string LandingHeader { get; set; }

        public string ChatHeader { get; set; }

        public string SelectListColor { get; set; }

        public string ListFontColor { get; set; }

        public string HeaderColor { get; set; }

        public string HeaderFontColor { get; set; }

        public string FontFamily { get; set; }

        public string OptionsHeaderColor { get; set; }

        public string OptionsHeaderFontColor { get; set; }

        public string ChatFontColor { get; set; }

        public string ChatBubbleColor { get; set; }

        public string ButtonColor { get; set; }

        public string ButtonFontColor { get; set; }

        public bool WidgetState { get; set; }

        public WidgetPreference()
        {
        }

        private WidgetPreference(
            string headerFontColor,
            string listFontColor,
            string selectListColor,
            string headerColor,
            string fontFamily,
            string landingHeader,
            string chatHeader,
            string placeholder,
            string accountId,
            bool widgetState,
            string optionsHeaderColor,
            string optionsHeaderFontColor,
            string chatFontColor,
            string chatBubbleColor,
            string buttonColor,
            string buttonFontColor
        )
        {
            HeaderFontColor = headerFontColor;
            ListFontColor = listFontColor;
            SelectListColor = selectListColor;
            HeaderColor = headerColor;
            FontFamily = fontFamily;
            LandingHeader = landingHeader;
            ChatHeader = chatHeader;
            Placeholder = placeholder;
            AccountId = accountId;
            WidgetState = widgetState;
            OptionsHeaderColor = optionsHeaderColor;
            OptionsHeaderFontColor = optionsHeaderFontColor;
            ChatFontColor = chatFontColor;
            ChatBubbleColor = chatBubbleColor;
            ButtonColor = buttonColor;
            ButtonFontColor = buttonFontColor;
        }

        public static WidgetPreference CreateNew(
            string headerFontColor,
            string listFontColor,
            string selectListColor,
            string headerColor,
            string fontFamily,
            string landingHeader,
            string chatHeader,
            string placeholder,
            string accountId,
            bool widgetState,
            string optionsHeaderColor,
            string optionsHeaderFontColor,
            string chatFontColor,
            string chatBubbleColor,
            string buttonColor,
            string buttonFontColor
        )
        {
            return new WidgetPreference(
                headerFontColor, listFontColor, selectListColor, headerColor, fontFamily,
                landingHeader, chatHeader, placeholder, accountId, widgetState,
                optionsHeaderColor,
                optionsHeaderFontColor,
                chatFontColor,
                chatBubbleColor,
                buttonColor,
                buttonFontColor
            );
        }

        public static WidgetPreference CreateDefault(string accountId)
        {
            var headerFontColor = "#191717";
            var listFontColor = "#100F0F";
            var selectListColor = "##F5F1F1";
            var headerColor = "#DBE3E3";
            var fontFamily = "Architects Daughter";
            var landingHeader = "<h2>Welcome!</h2>";
            var chatHeader = "<h2>Dogs Galore</h2><h4>We're the Dog Experts!</h4>";
            var placeholder = "Write here...";
            var optionsHeaderColor = "#37DCDC";
            var optionsHeaderFontColor = "#0E0909";
            var chatFontColor = "#080D0E";
            var chatBubbleColor = "#EFE5E5";
            var buttonColor = "#FFFFFF";
            var buttonFontColor = "#080D0E";

            return new WidgetPreference(
                headerFontColor, listFontColor, selectListColor, headerColor, fontFamily, landingHeader, chatHeader,
                placeholder, accountId, false,
                optionsHeaderColor,
                optionsHeaderFontColor,
                chatFontColor,
                chatBubbleColor,
                buttonColor,
                buttonFontColor
            );
        }

        public static WidgetPreference CreateEmpty(string accountId)
        {
            return new WidgetPreference(
                null, null, null, null, null,
                null, null, null, accountId, false, null, null, null,
                null, null, null);
        }
    }
}