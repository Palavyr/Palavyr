using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Requests;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Resources
{
    public interface IResponseCustomizer
    {
        Task<string> Customize(string html, EmailRequest request);
    }

    public class ResponseCustomizer : IResponseCustomizer
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly ILinkCreator linkCreator;
        private readonly IEntityStore<Logo> logoStore;

        public ResponseCustomizer(
            IEntityStore<Account> accountStore,
            ILinkCreator linkCreator,
            IEntityStore<Logo> logoStore
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
            var logo = await logoStore.GetOrNull(accountStore.AccountId, x => x.AccountId);


            if (logo is null || string.IsNullOrWhiteSpace(logo.AccountLogoFileId))
            {
                return html.Replace(ResponseVariableDefinition.LogoUriVariablePattern, string.Empty);
            }
            else
            {
                var link = await linkCreator.CreateLink(logo.AccountLogoFileId);
                var imgTag = $"<img src=\"{link}\" alt=\"Logo\" />";
                var updatedHtml = html.Replace(ResponseVariableDefinition.LogoUriVariablePattern, imgTag);
                return updatedHtml;
            }
        }
    }
}