using Palavyr.Domain.Accounts.Schemas;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.Domain.Resources.Responses
{
    public interface IResponseCustomizer
    {
        string Customize(string html, EmailRequest request, Account account);
        string CustomizeWithClientsName(string html, EmailRequest request);
        string CustomizeCompany(string html, Account account);
    }

    public class ResponseCustomizer : IResponseCustomizer
    {
        public string Customize(string html, EmailRequest request, Account account)
        {
            html = CustomizeWithClientsName(html, request);
            html = CustomizeCompany(html, account);
            html = CustomizeLogo(html, account);
            return html;
        }

        public string CustomizeWithClientsName(string html, EmailRequest request)
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

        public string CustomizeCompany(string html, Account account)
        {
            var companyName = string.IsNullOrWhiteSpace(account.CompanyName) ? "" : account.CompanyName;
            var updatedHtml = html.Replace(ResponseVariableDefinition.CompanyVariablePattern, companyName);
            return updatedHtml;
        }

        private string CustomizeLogo(string html, Account account)
        {
            var uri = string.IsNullOrWhiteSpace(account.AccountLogoUri) ? "" : account.AccountLogoUri;
            var link = string.IsNullOrWhiteSpace(uri) ? "" : $"<img src=\"{uri}\" alt=\"Logo\" />";
            var updatedHtml = html.Replace(ResponseVariableDefinition.LogoUriVariablePattern, link);
            return updatedHtml;
        }
    }
}