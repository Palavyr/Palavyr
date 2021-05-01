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
        private readonly IAttachmentSaver attachmentSaver;
        private ILogger<SaveMultipleAttachmentsController> logger;

        public SaveMultipleAttachmentsController(
            IConfiguration configuration,
            DashContext dashContext,
            IFileLinkRetriever fileLinkRetriever,
            IAttachmentSaver attachmentSaver,
            ILogger<SaveMultipleAttachmentsController> logger
        )
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.fileLinkRetriever = fileLinkRetriever;
            this.attachmentSaver = attachmentSaver;
            this.logger = logger;
        }

        [HttpPost("attachments/{areaId}/save-many")]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveMany(
            [FromRoute]
            string areaId,
            [FromHeader]
            string accountId,
            [FromForm(Name = "files")]
            IList<IFormFile> attachmentFiles)
        {
            var fileLinks = new List<FileLink>();
            foreach (var attachmentFile in attachmentFiles)
            {
                var fileLink = await attachmentSaver.SaveAttachment(accountId, areaId, attachmentFile);
                fileLinks.Add(fileLink);
            }

            return fileLinks.ToArray();
        }
    }
}