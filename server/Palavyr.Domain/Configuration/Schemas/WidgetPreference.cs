using System.ComponentModel.DataAnnotations;

namespace Palavyr.Domain.Configuration.Schemas
{
    public class WidgetPreference
    {
        [Key] public int? Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Placeholder { get; set; }

        public string AccountId { get; set; }

        public string Header { get; set; }

        public string SelectListColor { get; set; }

        public string ListFontColor { get; set; }

        public string HeaderColor { get; set; }

        public string HeaderFontColor { get; set; }

        public string FontFamily { get; set; }

        public string OptionsHeaderColor { get; set; }

        public string OptionsHeaderFontColor { get; set; }

        public string ChatFontColor { get; set; }

        public string ChatBubbleColor { get; set; }

        public bool WidgetState { get; set; }

        public WidgetPreference()
        {
        }

        private WidgetPreference(
            string headerFontColor, string listFontColor, string selectListColor,
            string headerColor, string fontFamily, string header, string title, string subtitle, string placeholder,
            string accountId, bool widgetState
        )
        {
            HeaderFontColor = headerFontColor;
            ListFontColor = listFontColor;
            SelectListColor = selectListColor;
            HeaderColor = headerColor;
            FontFamily = fontFamily;
            Header = header;
            Title = title;
            Subtitle = subtitle;
            Placeholder = placeholder;
            AccountId = accountId;
            WidgetState = widgetState;
        }

        public static WidgetPreference CreateNew(
            string headerFontColor, string listFontColor, string selectListColor,
            string headerColor, string fontFamily, string header, string title, string subtitle, string placeholder, string accountId, bool widgetState
        )
        {
            return new WidgetPreference(
                headerFontColor, listFontColor, selectListColor, headerColor, fontFamily,
                header, title, subtitle, placeholder, accountId, widgetState);
        }

        public static WidgetPreference CreateDefault(string accountId)
        {
            var headerFontColor = "#191717";
            var listFontColor = "#100F0F";
            var selectListColor = "##F5F1F1";
            var headerColor = "#DBE3E3";
            var fontFamily = "Architects Daughter";
            var header = "Welcome!";
            var title = "Tobies Galore";
            var subtitle = "Experts in Cavalier King Charles Spaniels";
            var placeholder = "Write here...";

            return new WidgetPreference(
                headerFontColor, listFontColor, selectListColor, headerColor, fontFamily, header, title, subtitle,
                placeholder, accountId, false);
        }

        public static WidgetPreference CreateEmpty(string accountId)
        {
            return new WidgetPreference(
                null, null, null, null, null,
                null, null, null, null, accountId, false);
        }
    }
}