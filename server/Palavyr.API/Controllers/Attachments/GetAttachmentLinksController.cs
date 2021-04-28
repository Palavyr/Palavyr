using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Attachments
{

    public class GetAttachmentLinksController : PalavyrBaseController
    {
        private readonly IConfiguration configuration;
        private DashContext dashContext;
        private ILogger<GetAttachmentLinksController> logger;
        private readonly IFileLinkRetriever fileLinkRetriever;

        public GetAttachmentLinksController(
            IConfiguration configuration,
            DashContext dashContext,
            ILogger<GetAttachmentLinksController> logger,
            IFileLinkRetriever fileLinkRetriever
        ) 
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.logger = logger;
            this.fileLinkRetriever = fileLinkRetriever;
        }
        
        [HttpGet("attachments/{areaId}")]
        public async Task<FileLink[]> Get([FromHeader] string accountId, string areaId)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;
            var fileLinks = await fileLinkRetriever.GetFileLinks(accountId, areaId, dashContext, logger, previewBucket);
            return fileLinks;
        }
    }
}