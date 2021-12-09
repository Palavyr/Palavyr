using System.Collections.Generic;
using System.Linq;
using System.Text;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService.PdfSections
{
    public static class ConversationDetailsSection
    {
        public static string GetConversationDetails(EmailRequest emailRequest, ICriticalResponses criticalResponses)
        {
            var userDetails = TwoColTableGenerator(GetUserDetails(emailRequest), "Your Details :");
            var convoDetails = TwoColTableGenerator(criticalResponses.CreateResponse(), "Your conversation details :");
            return $"<div style='padding-left: .5in; padding-right: .5in; width: 75%'>{userDetails}</div><div style='padding-left: .5in; padding-right: .5in; width: 75%' >{convoDetails}</div>";
        }

        private static List<Dictionary<string, string>> GetUserDetails(EmailRequest emailRequest)
        {
            var items = new List<Dictionary<string, string>>();
            
            if (!string.IsNullOrEmpty(emailRequest.Name))
                items.Add( new Dictionary<string, string>(){{"Name ", emailRequest.Name}});
                
            if (!string.IsNullOrEmpty(emailRequest.Phone)) 
                items.Add( new Dictionary<string, string>(){{"Phone ", emailRequest.Phone}});
            
            if (!string.IsNullOrEmpty(emailRequest.EmailAddress)) 
                items.Add( new Dictionary<string, string>(){{"Email ", emailRequest.EmailAddress}});
            
            items.Add(new Dictionary<string, string>(){{"Conversation Id ", emailRequest.ConversationId}});
            return items;
        }


        public static string TwoColTableGenerator(List<Dictionary<string, string>> items, string title)
        {
            var builder = new StringBuilder();
            if (items.Count > 0)
            {
                builder.Append(
                    @$"<span style='width: 50%; float: left; font-size: 18pt; padding-top: 2rem; padding-bottom: .5rem;' >{title}</span>"
                );
            }

            builder.Append(
                $@"<table style='table-layout:auto; width: 100%; padding border-collapse:collapse; text-align: left'><tbody>");

            foreach (var item in items)
            {
                var key = item.Keys.ToList()[0];
                var value = item.Values.ToList()[0];
                builder.Append(@$"<tr><td style='padding: 2mm' font-size: 10pt; scope='col'>{key}</td><td style='font-size: 12pt;' >{value}</td></tr>");
            }

            builder.Append(@"</tbody></table>");
            return builder.ToString();
        }
    }
}