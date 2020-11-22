using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystem.FormPaths;

namespace Palavyr.API.controllers.accounts.newAccount
{
    [Route("api")]
    [ApiController]
    public class GetCompanyLogoController : ControllerBase
    {
        private AccountsContext accountsContext;
        private ILogger<GetCompanyLogoController> logger;
        private IAmazonS3 s3Client;

        public GetCompanyLogoController(AccountsContext accountsContext, ILogger<GetCompanyLogoController> logger, IAmazonS3 s3Client)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.s3Client = s3Client;
        }
        
        [HttpGet("account/settings/logo")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            /// Do I upload an image file, or allow them to use a link?
            /// Only use an actual file for now
            var files = LogoPaths.ListLogoPathsAsDiskPaths(accountId);
            var logoFile = files[0]; // we only allow one logo file. If it changes, we delete it.
            var link = await UriUtils.CreateLogoImageLinkAsURI(
                logger,
                accountId,
                Path.GetFileName(logoFile),
                logoFile,
                s3Client
            );
            return Ok(link);
        }

        
    }
}