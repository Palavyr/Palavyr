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
            [FromForm(Name = "files")] IFormFile file)
        {
            await logoDeleter.DeleteLogo(accountId);
            var preSignedUrl = await logoSaver.SaveLogo(accountId, file);
            return preSignedUrl;
        }
    }
}