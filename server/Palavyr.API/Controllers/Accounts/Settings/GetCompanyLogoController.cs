using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.ListPaths;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetCompanyLogoController : PalavyrBaseController
    {
        private ILogger<GetCompanyLogoController> logger;
        private readonly ILinkCreator linkCreator;
        private readonly IConfiguration configuration;

        public GetCompanyLogoController(
            ILogger<GetCompanyLogoController> logger, 
            ILinkCreator linkCreator,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.linkCreator = linkCreator;
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
            var link = await linkCreator.CreateLogoImageLinkAsUri(
                accountId,
                Path.GetFileName(logoFile),
                logoFile,
                previewBucket
            );
            return Ok(link);
        }
    }
}