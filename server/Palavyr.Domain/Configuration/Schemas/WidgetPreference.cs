using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Palavyr.Domain.Configuration.Schemas
{
    public class WidgetPreference
    {
        [Key] public int? Id { get; set; }
        
        [DefaultValue("Tobies Galore")]
        public string Title { get; set; }

        [DefaultValue("Experts in Cavalier King Charles Spaniels")]
        public string Subtitle { get; set; }

        [DefaultValue("Write here...")]
        public string Placeholder { get; set; }
        
        public bool ShouldGroup { get; set; }
        
        public string AccountId { get; set; }
        
        [DefaultValue("Welcome!")]
        public string Header { get; set; }
        
        [DefaultValue("#E1E1E1")]
        public string SelectListColor { get; set; }
        
        [DefaultValue("red")]
        public string ListFontColor { get; set; }
        
        [DefaultValue("#35CCE6")]
        public string HeaderColor { get; set; }
        
        [DefaultValue("black")]
        public string HeaderFontColor { get; set; }
        
        [DefaultValue("Architects Daughter")]
        public string FontFamily { get; set; }
        
        [DefaultValue("#35CCE6")]
        public string OptionsHeaderColor { get; set; }
        
        [DefaultValue("black")]
        public string OptionsHeaderFontColor { get; set; }
        
        [DefaultValue("#35CCE6")]
        public string ChatFontColor { get; set; }
        
        [DefaultValue("#35CCE6")]
        public string ChatBubbleColor { get; set; }
        
        public bool WidgetState { get; set; }
        
        public WidgetPreference()
        {
        }

        private WidgetPreference(string headerFontColor, string listFontColor, string selectListColor,
            string headerColor, string fontFamily, string header, string title, string subtitle, string placeholder,
            bool shouldGroup, string accountId, bool widgetState)
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
            ShouldGroup = shouldGroup;
            AccountId = accountId;
            WidgetState = widgetState;
        }

        public static WidgetPreference CreateNew(string headerFontColor, string listFontColor, string selectListColor,
            string headerColor, string fontFamily, string header, string title, string subtitle, string placeholder,
            bool shouldGroup, string accountId, bool widgetState)
        {
            return new WidgetPreference(headerFontColor, listFontColor, selectListColor, headerColor, fontFamily,
                header, title, subtitle, placeholder, shouldGroup, accountId, widgetState);
        }
    }
}