namespace Palavyr.API.response
{
    public static class SupportedSubstitutions
    {
        private const string CompanyVariable = "{%Company%}";
        private const string NameVariable = "{%Name%}";
        private const string LogoUriVariable = "{%Logo%}";

        public static string MakeVariableSubstitutions(string html, string? companyName, string? clientName, string? logoUri)
        {
            html = companyName != null ? html.Replace(CompanyVariable, companyName) : html.Replace(CompanyVariable, "");
            html = html.Replace(NameVariable, clientName ?? "");
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