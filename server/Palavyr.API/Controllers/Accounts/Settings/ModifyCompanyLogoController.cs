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
            [FromForm(Name = "files")] IFormFile file,
            CancellationToken cancellationToken
            )
        {
            await logoDeleter.DeleteLogo();
            var preSignedUrl = await logoSaver.SaveLogo(file);
            return preSignedUrl;
        }

        [HttpDelete("account/settings/logo")]
        public async Task Delete(CancellationToken cancellationToken)
        {
            await logoDeleter.DeleteLogo();
        }
    }
}