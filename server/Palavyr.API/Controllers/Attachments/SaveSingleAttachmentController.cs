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
        private readonly IAttachmentSaver attachmentSaver;

        public SaveSingleAttachmentController(
            IConfiguration configuration,
            ILogger<SaveSingleAttachmentController> logger,
            IFileLinkRetriever fileLinkRetriever,
            DashContext dashContext,
            IAttachmentSaver attachmentSaver
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.fileLinkRetriever = fileLinkRetriever;
            this.dashContext = dashContext;
            this.attachmentSaver = attachmentSaver;
        }

        [HttpPost("attachments/{areaId}/save-one")]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveSingle(
            [FromHeader]
            string accountId,
            [FromRoute]
            string areaId,
            [FromForm(Name = "files")]
            IFormFile attachmentFile)
        {
            var fileLink = await attachmentSaver.SaveAttachment(accountId, areaId, attachmentFile);
            return new[] {fileLink};
           
        }
    }
}