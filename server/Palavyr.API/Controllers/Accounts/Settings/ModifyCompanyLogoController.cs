using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Services.LogoServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class ModifyCompanyLogoController : PalavyrBaseController
    {
        private readonly ILogoSaver logoSaver;
        private readonly ILogoDeleter logoDeleter;

        public ModifyCompanyLogoController(
            ILogoSaver logoSaver,
            ILogoDeleter logoDeleter
        )
        {
            this.logoSaver = logoSaver;
            this.logoDeleter = logoDeleter;
        }

        [HttpPut("account/settings/logo")]
        [ActionName("Decode")]
        public async Task<string> Modify(
            [FromHeader] string accountId,
            [FromForm(Name = "files")] IFormFile file,
            CancellationToken cancellationToken
            )
        {
            await logoDeleter.DeleteLogo(accountId, cancellationToken);
            var preSignedUrl = await logoSaver.SaveLogo(accountId, file, cancellationToken);
            return preSignedUrl;
        }

        [HttpDelete("account/settings/logo")]
        public async Task Delete(
            [FromHeader]
            string accountId,
            CancellationToken cancellationToken)
        {
            await logoDeleter.DeleteLogo(accountId, cancellationToken);
        }
    }
}