using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Models.Resources.Responses
{
    public interface IResponseCustomizer
    {
        Task<string> Customize(string html, EmailRequest request);
    }

    public class ResponseCustomizer : IResponseCustomizer
    {
        private readonly IConfigurationEntityStore<Account> accountStore;
        private readonly ILinkCreator linkCreator;
        private readonly IConfigurationEntityStore<Logo> logoStore;

        public ResponseCustomizer(
            IConfigurationEntityStore<Account> accountStore,
            ILinkCreator linkCreator,
            IConfigurationEntityStore<Logo> logoStore
        )
        {
            this.accountStore = accountStore;
            this.linkCreator = linkCreator;
            this.logoStore = logoStore;
        }

        public async Task<string> Customize(string html, EmailRequest request)
        {
            html = CustomizeWithClientsName(html, request);
            html = await CustomizeCompany(html);
            html = await CustomizeLogo(html);
            return html;
        }

        private string CustomizeWithClientsName(string html, EmailRequest request)
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


        private async Task<string> CustomizeCompany(string html)
        {
            var account = await accountStore.Get(accountStore.AccountId, x => x.AccountId);
            var companyName = string.IsNullOrWhiteSpace(account.CompanyName) ? "" : account.CompanyName;
            var updatedHtml = html.Replace(ResponseVariableDefinition.CompanyVariablePattern, companyName);
            return updatedHtml;
        }

        private async Task<string> CustomizeLogo(string html)
        {
            var logo = await logoStore.Get(accountStore.AccountId, x => x.AccountId);
            var link = await linkCreator.CreateLink(logo.AccountLogoFileId);


            if (string.IsNullOrWhiteSpace(link))
            {
                return html.Replace(ResponseVariableDefinition.LogoUriVariablePattern, string.Empty);
            }
            else
            {
                var imgTag = $"<img src=\"{link}\" alt=\"Logo\" />";
                var updatedHtml = html.Replace(ResponseVariableDefinition.LogoUriVariablePattern, imgTag);
                return updatedHtml;
            }
        }
    }
}