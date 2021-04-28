using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.FormPaths;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Attachments
{
    public class SaveSingleAttachmentController : PalavyrBaseController
    {
        private ILogger<SaveSingleAttachmentController> logger;
        private readonly IFileLinkRetriever fileLinkRetriever;
        private readonly IConfiguration configuration;
        private readonly DashContext dashContext;

        public SaveSingleAttachmentController(
            IConfiguration configuration,
            ILogger<SaveSingleAttachmentController> logger,
            IFileLinkRetriever fileLinkRetriever,
            DashContext dashContext
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.fileLinkRetriever = fileLinkRetriever;
            this.dashContext = dashContext;
        }

        [HttpPost("attachments/{areaId}/save-one")]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveSingle(
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

            var fileLinks = await fileLinkRetriever.GetFileLinks(accountId, areaId, dashContext, logger, previewBucket);
            return fileLinks;
        }
    }
}