


using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystem.FormPaths;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class SaveSingleAttachmentController : AttachmentsBase
    {
        private ILogger<SaveSingleAttachmentController> logger;
        private readonly IAmazonS3 s3Client;
        private readonly DashContext dashContext;

        public SaveSingleAttachmentController(
            ILogger<SaveSingleAttachmentController> logger,
            IAmazonS3 s3Client, 
            DashContext dashContext
            )
        {
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
            
            var fileLinks = await GetFileLinks(accountId, areaId, dashContext, logger, s3Client);
            return Ok(fileLinks);
        }
    }
}