using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.API.Controllers.Attachments
{
    public interface IAttachmentRetriever
    {
        Task<FileLink[]> RetrieveAttachments(string account, string areaId, CancellationToken cancellationToken);
    }

    public class AttachmentRetriever : IAttachmentRetriever
    {
        private readonly DashContext dashContext;
        private readonly IConfiguration configuration;
        private readonly ILinkCreator linkCreator;

        public AttachmentRetriever(
            DashContext dashContext,
            IConfiguration configuration,
            ILinkCreator linkCreator
        )
        {
            this.dashContext = dashContext;
            this.configuration = configuration;
            this.linkCreator = linkCreator;
        }

        public async Task<FileLink[]> RetrieveAttachments(string account, string areaId, CancellationToken cancellationToken)
        {
            var userDataBucket = configuration.GetSection(ConfigSections.UserDataSection).Value;
            var metas = await dashContext.FileNameMaps
                .Where(x => x.AreaIdentifier == areaId)
                .Select(
                    x => new AttachmentMeta
                    {
                        SafeFileId = x.SafeName,
                        S3Key = x.S3Key,
                        RiskyName = x.RiskyName
                    }).ToListAsync(cancellationToken);

            var fileLinks = new List<FileLink>();
            foreach (var meta in metas)
            {
                var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(meta.S3Key, userDataBucket);
                fileLinks.Add(FileLink.CreateLink(meta.RiskyName, preSignedUrl, meta.SafeFileId));
            }

            return fileLinks.ToArray();
        }
    }
}