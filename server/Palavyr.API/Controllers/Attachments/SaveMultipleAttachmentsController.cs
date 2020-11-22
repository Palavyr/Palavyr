using System;
using System.Collections.Generic;
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
    public class SaveMultipleAttachmentsController : AttachmentsBase
    {
        private DashContext dashContext;
        private ILogger<SaveMultipleAttachmentsController> logger;
        private readonly IAmazonS3 s3Client;

        public SaveMultipleAttachmentsController(
            DashContext dashContext,
            IAmazonS3 s3Client,
            ILogger<SaveMultipleAttachmentsController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
            this.s3Client = s3Client;
        }

        [HttpPost("attachments/{areaId}/save-many")]
        [ActionName("Decode")]
        public async Task<IActionResult> SaveMany(
            [FromRoute] string areaId, 
            [FromHeader] string accountId,
            [FromForm(Name = "files")] IList<IFormFile> attachmentFiles)
        {
            // TODO write filename only to the database, then generate GUID to use as filename, then save, then use the db map of guid to filename to get the file.
            var attachmentDir = FormDirectoryPaths.FormAttachmentDirectoryWithCreate(accountId, areaId);
            foreach (var formFile in attachmentFiles)
            {
                logger.LogDebug($"File name write: {formFile.FileName}");
                var safeFileName = Guid.NewGuid().ToString() + ".pdf";
                var riskyFileName = formFile.FileName;

                var fileMap = FileNameMap.CreateFileMap(safeFileName, riskyFileName, accountId, areaId);
                await dashContext.FileNameMaps.AddAsync(fileMap);

                var fileSavePath = Path.Combine(attachmentDir, safeFileName);
                using var fileStream = new FileStream(fileSavePath, FileMode.Create);
                await formFile.CopyToAsync(fileStream);
            }

            await dashContext.SaveChangesAsync();
            var fileLinks = await GetFileLinks(accountId, areaId, dashContext, logger, s3Client);
            return Ok(fileLinks);
        }
    }
}