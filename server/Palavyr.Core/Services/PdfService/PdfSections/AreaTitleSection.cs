﻿using System.Text;

namespace Palavyr.Core.Services.PdfService.PdfSections
{
    public static class AreaTitleSection
    {
        
        private static string CreateAreaDisplayTitle(string title, string conversationId)
        {            
            var builder = new StringBuilder();
            builder.Append($@"<section id='TITLE' style='padding-top: 2.2rem; text-align: center; margin-bottom: 10mm;'>");
            builder.Append($@"<h2>{title}</h2>");
            builder.Append($@"<h5>Conversation Id: {conversationId}</h5>");
            builder.Append($@"</section>");
            
            return builder.ToString();
        }

        public static string GetAreaDisplayTitle(string title, string conversationId)
        {
            return CreateAreaDisplayTitle(title, conversationId);
        }
    }
}