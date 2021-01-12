using System.Linq;
using Palavyr.API.RequestTypes;
using Server.Domain.Accounts;

namespace Palavyr.API.Response
{
    public static class ResponseCustomizer
    {
        public static string Customize(string html, EmailRequest request, UserAccount account)
        {
            html = CustomizeName(html, request);
            html = CustomizeCompany(html, account);
            html = CustomizeLogo(html, account);
            return html;
        }

        public static string CustomizeName(string html, EmailRequest request)
        {
            var nameElement = request.KeyValues.SingleOrDefault(dict => dict.ContainsKey(ResponseVariableDefinition.Name));

            var customName = "";
            if (nameElement != null)
            {
                customName = nameElement.TryGetValue("Name", out var clientName) ? clientName : "";
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