#nullable enable
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
        public string? Get(
            [FromHeader] string accountId)
        {
            var preSignedUrl = logoRetriever.GetLogo(accountId);
            return preSignedUrl;
        }
    }
}