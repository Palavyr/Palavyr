using DashboardServer.Data;

namespace Palavyr.API.GeneratePdf
{
    public static class SupportedSubstitutions
    {
        private const string CompanyVariable = "{%Company%}";
        private const string NameVariable = "{%Name%}";
        private const string LogoUriVariable = "{%Logo%}";

        public static string MakeVariableSubstitutions(string html, string? companyName, string? clientName, string? logoUri)
        {
            if (companyName != null)
                html = html.Replace(CompanyVariable, companyName);
            else
                html = html.Replace(CompanyVariable, "");
            
            if (clientName != null)
                html = html.Replace(NameVariable, clientName);
            else
                html = html.Replace(NameVariable, "");
            
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