﻿namespace Palavyr.API.RequestTypes
{
    public class Text
    {
        public string EmailTemplate { get; set; }
        public string FileName { get; set; }
        public string FileId { get; set; }
        public string GroupName { get; set; }
        public string ParentGroup { get; set; }
        
        public string AreaName { get; set; }
        public string AreaDisplayTitle { get; set; }
    }

    public class SubjectText
    {
        public string Subject { get; set; }
    }
}