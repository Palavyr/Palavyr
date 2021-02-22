using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.Common.GlobalConstants;
using Palavyr.Data;

namespace Palavyr.API.Controllers.Attachments
{
    [Route("api")]
    [ApiController]
    public class DeleteAttachmentLinksController : AttachmentsBase
    {
        private readonly IConfiguration configuration;
        private DashContext dashContext;
        private ILogger<DeleteAttachmentLinksController> logger;
        private IAmazonS3 s3Client;

        public DeleteAttachmentLinksController(
            IConfiguration configuration,
            DashContext dashContext,
            ILogger<DeleteAttachmentLinksController> logger,
            IAmazonS3 s3Client
        )
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.logger = logger;
            this.s3Client = s3Client;
        }
            
        [HttpDelete("attachments/{areaId}/file-link")]
        public async Task<IActionResult> Delete([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] Text text)
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
            
            var fileLinks = await GetFileLinks(accountId, areaId, dashContext, logger, s3Client, previewBucket);
            return Ok(fileLinks.ToArray());
        }
    }
}