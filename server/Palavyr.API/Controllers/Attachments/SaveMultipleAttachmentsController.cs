using System;
using System.Collections.Generic;
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

    public class SaveMultipleAttachmentsController : PalavyrBaseController
    {
        private readonly IConfiguration configuration;
        private DashContext dashContext;
        private readonly IFileLinkRetriever fileLinkRetriever;
        private ILogger<SaveMultipleAttachmentsController> logger;

        public SaveMultipleAttachmentsController(
            IConfiguration configuration,
            DashContext dashContext,
            IFileLinkRetriever fileLinkRetriever,
            ILogger<SaveMultipleAttachmentsController> logger
        )
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.fileLinkRetriever = fileLinkRetriever;
            this.logger = logger;
        }

        [HttpPost("attachments/{areaId}/save-many")]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveMany(
            [FromRoute] string areaId, 
            [FromHeader] string accountId,
            [FromForm(Name = "files")] IList<IFormFile> attachmentFiles)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;

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
            var fileLinks = await fileLinkRetriever.GetFileLinks(accountId, areaId, dashContext, logger, previewBucket);
            return fileLinks;
        }
    }
    
}