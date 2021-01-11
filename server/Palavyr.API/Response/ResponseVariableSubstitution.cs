using System.Collections.Generic;

namespace Palavyr.API.Response
{
    public class ResponseVariable
    {
        public string Name { get; set; }
        public string Details { get; set; }

        public ResponseVariable(string name, string details)
        {
            Name = name;
            Details = details;
        }
    }
    
    public static class ResponseVariableSubstitution
    {
        private const string CompanyVariable = @"{%Company%}";
        private const string NameVariable = @"{%Name%}";
        private const string LogoUriVariable = @"{%Logo%}";

        public static List<ResponseVariable> GetAvailableVariables()
        {
            return new List<ResponseVariable>()
            {
                new ResponseVariable(CompanyVariable, "Your company name provided in Settings."),
                new ResponseVariable(NameVariable, "The name provided by the client in the chat dialog."),
                new ResponseVariable(LogoUriVariable, "The logo you provided in Settings"),
            };
        }
                
        public static string MakeVariableSubstitutions(string html, string? companyName, string? clientName, string? logoUri)
        {
            html = html.Replace(CompanyVariable, string.IsNullOrWhiteSpace(companyName) ? "" : companyName);
            html = html.Replace(NameVariable, string.IsNullOrWhiteSpace(clientName) ? "" : clientName);
            if (logoUri != null)
            {
                var logo = "Logo";
                var link = $"<img src={logoUri} alt={logo} />";
                html = html.Replace(LogoUriVariable, link);
            }
            else
            {
                html = html.Replace(LogoUriVariable, "");
            }

            return html;
        }
    }
}