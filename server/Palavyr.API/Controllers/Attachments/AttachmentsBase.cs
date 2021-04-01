using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.ListPaths;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.API.Controllers.Attachments
{
    public abstract class AttachmentsBase : PalavyrBaseController
    {
        [NonAction]
        public async Task<FileLink[]> GetFileLinks(
            string accountId,
            string areaId,
            DashContext dashContext,
            ILogger logger,
            IAmazonS3 s3Client,
            string previewBucket)
        {
            var files = AttachmentPaths.GetAttachmentFileList(accountId, areaId);

            var links = new List<FileLink>();
            foreach (var fi in files)
            {
                logger.LogDebug($"File: {fi}");
                var fileMap = dashContext.FileNameMaps.Single(row => row.SafeName == fi.Name);
                var link = await UriUtils.CreateAttachmentLinkAsURI(logger, accountId, areaId, fileMap.SafeName, s3Client, previewBucket);
                links.Add(FileLink.CreateLink(fileMap.RiskyName, link, fileMap.SafeName));
            }

            return links.ToArray();
        }
    }
}