
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.AccountServices.PlanTypes;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.AttachmentServices
{
    public interface IAttachmentRetriever
    {
        Task<FileAsset[]> GetAttachmentLinksForIntent(string intentId);
        Task<IHaveBeenDownloadedFromCloudToLocal[]> GatherAttachments(string intentId, List<CloudFileDownloadRequest>? additionalFiles = null);
    }

    public class AttachmentRetriever : IAttachmentRetriever
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly ICloudFileDownloader cloudFileDownloader;
        private readonly IBusinessRules businessRules;

        public AttachmentRetriever(
            IEntityStore<Intent> intentStore,
            IEntityStore<FileAsset> fileAssetStore,
            ICloudFileDownloader cloudFileDownloader,
            IBusinessRules businessRules
        )
        {
            this.intentStore = intentStore;
            this.fileAssetStore = fileAssetStore;
            this.cloudFileDownloader = cloudFileDownloader;
            this.businessRules = businessRules;
        }

        public async Task<IHaveBeenDownloadedFromCloudToLocal[]> GatherAttachments(string intentId, List<CloudFileDownloadRequest>? additionalFiles = null)
        {
            var s3DownloadRequestMetas = await RetrievePdfUris(intentId);

            if (additionalFiles != null)
            {
                s3DownloadRequestMetas.AddRange(additionalFiles);
            }

            var attachments = await DownloadForAttachmentToEmail(s3DownloadRequestMetas);
            return attachments;
        }

        public async Task<FileAsset[]> GetAttachmentLinksForIntent(string intentId)
        {
            var intent = await intentStore.Query().Include(x => x.AttachmentRecords).SingleAsync(x => x.IntentId == intentId, intentStore.CancellationToken);
            var attachmentFileIds = intent.AttachmentRecords.Select(x => x.FileId).ToArray();
            var fileAssets = await fileAssetStore.Query().Where(x => attachmentFileIds.Contains(x.FileId)).ToListAsync(fileAssetStore.CancellationToken);

            return fileAssets.ToArray();
        }

        private async Task<List<CloudFileDownloadRequest>> RetrievePdfUris(string intentId)
        {
            var intent = await intentStore.Query()
                .Where(x => x.IntentId == intentId)
                .Include(x => x.AttachmentRecords)
                .SingleAsync(intentStore.CancellationToken);
            var attachmentFileIds = intent.AttachmentRecords.Select(x => x.FileId).ToArray();
            var fileAssets = await fileAssetStore.Query().Where(x => attachmentFileIds.Contains(x.FileId)).ToListAsync(fileAssetStore.CancellationToken);

            var metas = new List<CloudFileDownloadRequest>();

            foreach (var asset in fileAssets)
            {
                metas.Add(
                    new CloudFileDownloadRequest
                    {
                        LocationKey = asset.LocationKey,
                        FileNameWithExtension = string.Join("", asset.RiskyNameStem, asset.Extension)
                    });
            }

            return metas;
        }


        private async Task<IHaveBeenDownloadedFromCloudToLocal[]> DownloadForAttachmentToEmail(List<CloudFileDownloadRequest> cloudFileDownloadRequests)
        {
            var localFilePaths = await cloudFileDownloader.DownloadObjectsFromS3(cloudFileDownloadRequests);
            if (localFilePaths == null)
            {
                throw new DomainException("Unable to download attachmentFiles");
            }

            var totalAttachmentsAllowed = await businessRules.GetAllowedAttachments();
            return localFilePaths.Take(totalAttachmentsAllowed).ToArray();
        }
    }
}