﻿using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Configuration.schema
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
        public string Header { get; set; }
        public string SelectListColor { get; set; }
        public string HeaderColor { get; set; }
        public string FontFamily { get; set; }
        public string HeaderFontColor { get; set; }
        public string ListFontColor { get; set; }
        
        
        WidgetPreference() { }
        
        WidgetPreference(string headerFontColor, string listFontColor, string selectListColor, string headerColor, string fontFamily, string header, string title, string subtitle, string placeholder, bool shouldGroup, string accountId)
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
        }

        public static WidgetPreference CreateNew(string headerFontColor, string listFontColor, string selectListColor, string headerColor, string fontFamily, string header, string title, string subtitle, string placeholder, bool shouldGroup, string accountId)
        {
            return new WidgetPreference(headerFontColor, listFontColor, selectListColor, headerColor, fontFamily, header, title, subtitle, placeholder, shouldGroup, accountId);
        }
    }
}