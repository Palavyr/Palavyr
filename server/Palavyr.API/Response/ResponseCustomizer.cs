using Palavyr.API.RequestTypes;
using Palavyr.Domain.Accounts.Schemas;

namespace Palavyr.API.Response
{
    public static class ResponseCustomizer
    {
        public static string Customize(string html, EmailRequest request, UserAccount account)
        {
            html = CustomizeWithClientsName(html, request);
            html = CustomizeCompany(html, account);
            html = CustomizeLogo(html, account);
            return html;
        }

        public static string CustomizeWithClientsName(string html, EmailRequest request)
        {
            var nameElement = request.Name;
            var customName = "";
            if (nameElement != null)
            {
                customName = nameElement;
            }

            html = html.Replace(ResponseVariableDefinition.NameVariablePattern, customName);
            return html;
        }

        public static string CustomizeCompany(string html, UserAccount account)
        {
            var companyName = string.IsNullOrWhiteSpace(account.CompanyName) ? "" : account.CompanyName;
            var updatedHtml = html.Replace(ResponseVariableDefinition.CompanyVariablePattern, companyName);
            return updatedHtml;
        }

        private static string CustomizeLogo(string html, UserAccount account)
        {
            var uri = string.IsNullOrWhiteSpace(account.AccountLogoUri) ? "" : account.AccountLogoUri;
            var link = string.IsNullOrWhiteSpace(uri) ? "" : $"<img src=\"{uri}\" alt=\"Logo\" />";
            var updatedHtml = html.Replace(ResponseVariableDefinition.LogoUriVariablePattern, link);
            return updatedHtml;
        }
    }
}