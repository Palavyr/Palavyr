using System.ComponentModel.DataAnnotations;

namespace Server.Domain
{
    public class WidgetPreference
    {
        [Key] 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Placeholder { get; set; }
        public bool ShouldGroup { get; set; }
        public string AccountId { get; set; }

        WidgetPreference(string title, string subtitle, string placeholder, bool shouldGroup, string accountId)
        {
            Title = title;
            Subtitle = subtitle;
            Placeholder = placeholder;
            ShouldGroup = shouldGroup;
            AccountId = accountId;
        }

        public static WidgetPreference CreateNew(string title, string subtitle, string placeholder, bool shouldGroup, string accountId)
        {
            return new WidgetPreference(title, subtitle, placeholder, shouldGroup, accountId);
        }
    }
}