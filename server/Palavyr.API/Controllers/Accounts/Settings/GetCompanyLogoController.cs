using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystemTools.ListPaths;
using Palavyr.Common.GlobalConstants;
using Palavyr.Services.AmazonServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetCompanyLogoController : PalavyrBaseController
    {
        private ILogger<GetCompanyLogoController> logger;
        private IAmazonS3 s3Client;
        private readonly IConfiguration configuration;

        public GetCompanyLogoController(
            ILogger<GetCompanyLogoController> logger, 
            IAmazonS3 s3Client,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.s3Client = s3Client;
            this.configuration = configuration;
        }
        
        [HttpGet("account/settings/logo")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;
            
            /// Do I upload an image file, or allow them to use a link?
            /// Only use an actual file for now
            var files = LogoPaths.ListLogoPathsAsDiskPaths(accountId);
            var logoFile = files.FirstOrDefault(); // we only allow one logo file. If it changes, we delete it.
            if (logoFile == null)// if no logo uploaded
            {
                return Ok(null);
            }
            var link = await UriUtils.CreateLogoImageLinkAsURI(
                logger,
                accountId,
                Path.GetFileName(logoFile),
                logoFile,
                s3Client,
                previewBucket
            );
            return Ok(link);
        }
    }
}