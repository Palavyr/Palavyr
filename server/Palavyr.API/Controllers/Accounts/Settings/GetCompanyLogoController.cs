#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Services.LogoServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetCompanyLogoController : PalavyrBaseController
    {
        private readonly ILogoRetriever logoRetriever;

        public GetCompanyLogoController(
            ILogoRetriever logoRetriever
        )
        {
            this.logoRetriever = logoRetriever;
        }

        [HttpGet("account/settings/logo")]
        public async Task<string?> Get(
            [FromHeader]
            string accountId,
            CancellationToken cancellationToken)
        {
            var preSignedUrl = await logoRetriever.GetLogo(accountId, cancellationToken);
            return preSignedUrl;
        }
    }
}