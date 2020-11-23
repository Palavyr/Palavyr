using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.ResponseTypes;
using Palavyr.Common.FileSystem.FormPaths;

namespace Palavyr.API.Controllers
{
    public abstract class AttachmentsBase : ControllerBase
    {
        [NonAction] 
        public async Task<FileLink[]> GetFileLinks(string accountId, string areaId, DashContext dashContext, ILogger logger, IAmazonS3 s3Client)
        {
            var files = AttachmentPaths.GetAttachmentFileList(accountId, areaId);

            var links = new List<FileLink>();
            foreach (var fi in files)
            {
                var fileMap = dashContext.FileNameMaps.Single(row => row.SafeName == fi.Name);
                var link = await UriUtils.CreateAttachmentLinkAsURI(logger, accountId, areaId, fileMap.SafeName, s3Client);
                links.Add(FileLink.CreateLink(fileMap.RiskyName, link, fileMap.SafeName));
            }

            return links.ToArray();
        }
    }
}