using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.ListPaths;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.API.Controllers.Attachments
{
    public interface IFileLinkRetriever
    {
        Task<FileLink[]> GetFileLinks(
            string accountId,
            string areaId,
            DashContext dashContext,
            ILogger logger,
            string previewBucket);
    }

    public class FileLinkRetriever : IFileLinkRetriever
    {
        private readonly ILinkCreator linkCreator;

        public FileLinkRetriever(ILinkCreator linkCreator)
        {
            this.linkCreator = linkCreator;
        }

        public async Task<FileLink[]> GetFileLinks(
            string accountId,
            string areaId,
            DashContext dashContext,
            ILogger logger,
            string previewBucket)
        {
            var files = AttachmentPaths.GetAttachmentFileList(accountId, areaId);
            var links = new List<FileLink>();
            foreach (var fi in files)
            {
                logger.LogDebug($"File: {fi}");
                var fileMap = dashContext.FileNameMaps.Single(row => row.SafeName == fi.Name);
                var link = await linkCreator.CreateAttachmentLinkAsUri(logger, accountId, areaId, fileMap.SafeName, previewBucket);
                links.Add(FileLink.CreateLink(fileMap.RiskyName, link, fileMap.SafeName));
            }
            return links.ToArray();
        }
    }
}