#nullable enable
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoRetriever
    {
        Task<string?> GetLogo();
        Task<string?> GetLogoThumbnail();
    }

    public class LogoRetriever : ILogoRetriever
    {
        private readonly IConfigurationEntityStore<Logo> logoStore;
        private readonly ILinkCreator linkCreator;

        public LogoRetriever(
            IConfigurationEntityStore<Logo> logoStore,
            ILinkCreator linkCreator
        )
        {
            this.logoStore = logoStore;
            this.linkCreator = linkCreator;
        }

        public async Task<string?> GetLogo()
        {
            var logo = await logoStore.Get(logoStore.AccountId, x => x.AccountId);
            if (string.IsNullOrEmpty(logo.AccountLogoFileId)) return null;
            var logoLink = await linkCreator.CreateLink(logo.AccountLogoFileId);
            return logoLink;
        }

        public Task<string?> GetLogoThumbnail()
        {
            throw new System.NotImplementedException();
        }
    }
}