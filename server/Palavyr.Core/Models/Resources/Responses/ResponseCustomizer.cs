using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Models.Resources.Responses
{
    public interface IResponseCustomizer
    {
        string Customize(string html, EmailRequest request, Account account);
        string CustomizeWithClientsName(string html, EmailRequest request);
        string CustomizeCompany(string html, Account account);
    }

    public class ResponseCustomizer : IResponseCustomizer
    {
        private readonly ILinkCreator linkCreator;
        private readonly IConfiguration configuration;

        public ResponseCustomizer(ILinkCreator linkCreator, IConfiguration configuration)
        {
            this.linkCreator = linkCreator;
            this.configuration = configuration;
        }

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
            if (string.IsNullOrWhiteSpace(account.AccountLogoUri))
            {
                return html;
            }
            else
            {
                var bucket = configuration.GetUserDataSection();
                var link = linkCreator.GenericCreatePreSignedUrl(account.AccountLogoUri, bucket);
                var imgTag = $"<img src=\"{link}\" alt=\"Logo\" />";
                var updatedHtml = html.Replace(ResponseVariableDefinition.LogoUriVariablePattern, imgTag);
                return updatedHtml;
            }
        }
    }
}