using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.FormPaths;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Attachments
{
    public class DeleteAttachmentLinksController : PalavyrBaseController
    {
        private readonly IConfiguration configuration;
        private DashContext dashContext;
        private ILogger<DeleteAttachmentLinksController> logger;
        private readonly IFileLinkRetriever fileLinkRetriever;

        public DeleteAttachmentLinksController(
            IConfiguration configuration,
            DashContext dashContext,
            ILogger<DeleteAttachmentLinksController> logger,
            IFileLinkRetriever fileLinkRetriever
        )
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.logger = logger;
            this.fileLinkRetriever = fileLinkRetriever;
        }

        [HttpDelete("attachments/{areaId}/file-link")]
        public async Task<FileLink[]> Delete([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] Text text)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;

            var filePath = FormFilePath.FormAttachmentFilePath(accountId, areaId, text.FileId);
            if (DiskUtils.ValidatePathExists(filePath))
            {
                logger.LogDebug($"Trying to delete file path: {filePath}");
                System.IO.File.Delete(filePath);
            }

            var entity = await dashContext.FileNameMaps.SingleOrDefaultAsync(row => row.SafeName == text.FileId);
            if (entity != null)
            {
                dashContext.FileNameMaps.Remove(entity);
                await dashContext.SaveChangesAsync();
            }

            var fileLinks = await fileLinkRetriever.GetFileLinks(accountId, areaId, dashContext, logger, previewBucket);
            return fileLinks;
        }
    }
}