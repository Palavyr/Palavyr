using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystemTools.FormPaths;
using Palavyr.Common.GlobalConstants;
using Palavyr.Data;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Attachments
{

    public class SaveSingleAttachmentController : AttachmentsBase
    {
        private ILogger<SaveSingleAttachmentController> logger;
        private readonly IConfiguration configuration;
        private readonly IAmazonS3 s3Client;
        private readonly DashContext dashContext;

        public SaveSingleAttachmentController(
            IConfiguration configuration,
            ILogger<SaveSingleAttachmentController> logger,
            IAmazonS3 s3Client, 
            DashContext dashContext
            )
        {
            this.configuration = configuration;
            this.s3Client = s3Client;
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPost("attachments/{areaId}/save-one")]
        [ActionName("Decode")]
        public async Task<IActionResult> SaveSingle(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromForm(Name = "files")] IFormFile attachmentFile)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;
            var attachmentDir = FormDirectoryPaths.FormAttachmentDirectoryWithCreate(accountId, areaId);
            var safeFileName = Guid.NewGuid() + ".pdf";
            var riskyFileName = attachmentFile.FileName;
            logger.LogDebug($"File name write: {attachmentFile.FileName}");

            var fileNameMap = FileNameMap.CreateFileMap(safeFileName, riskyFileName, accountId, areaId);

            await dashContext.FileNameMaps.AddAsync(fileNameMap); // must be async
            await dashContext.SaveChangesAsync();

            var fileSavePath = Path.Combine(attachmentDir, safeFileName);
            await using var fileStream = new FileStream(fileSavePath, FileMode.Create);
            await attachmentFile.CopyToAsync(fileStream);
            fileStream.Close();
            
            var fileLinks = await GetFileLinks(accountId, areaId, dashContext, logger, s3Client, previewBucket);
            return Ok(fileLinks);
        }
    }
}