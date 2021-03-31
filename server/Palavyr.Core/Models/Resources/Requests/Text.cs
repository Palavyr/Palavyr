namespace Palavyr.Core.Models.Resources.Requests
{
    public class Text
    {
        public string EmailTemplate { get; set; }
        public string FileName { get; set; }
        public string FileId { get; set; }
        public string GroupName { get; set; }
        public string ParentGroup { get; set; }
    }

    public class SubjectText
    {
        public string Subject { get; set; }
    }

    public class AreaNameText
    {
        public string AreaName { get; set; }
    }

    public class AreaDisplayTitleText
    {
        public string AreaDisplayTitle { get; set; }
    }
}