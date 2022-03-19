#nullable enable
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoRetriever
    {
        Task<string?> GetLogo();
        Task<string?> GetLogoThumbnail();
    }

    public class LogoRetriever : ILogoRetriever
    {
        private readonly IEntityStore<Logo> logoStore;
        private readonly ILinkCreator linkCreator;

        public LogoRetriever(
            IEntityStore<Logo> logoStore,
            ILinkCreator linkCreator
        )
        {
            this.logoStore = logoStore;
            this.linkCreator = linkCreator;
        }

        public async Task<string?> GetLogo()
        {
            var logo = await logoStore.GetOrNull(logoStore.AccountId, x => x.AccountId);
            if (logo is null || string.IsNullOrEmpty(logo.AccountLogoFileId)) return null;
            var logoLink = await linkCreator.CreateLink(logo.AccountLogoFileId);
            return logoLink;
        }

        public Task<string?> GetLogoThumbnail()
        {
            throw new System.NotImplementedException();
        }
    }
}