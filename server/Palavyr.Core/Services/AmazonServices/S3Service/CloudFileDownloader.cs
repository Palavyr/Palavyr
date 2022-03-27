#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface ICloudFileDownloader
    {
        Task<IHaveBeenDownloadedFromCloudToLocal[]?> DownloadObjectsFromS3(List<CloudFileDownloadRequest> metas);
    }

    public class CloudFileDownloader : ICloudFileDownloader
    {
        private readonly IS3Downloader s3Downloader;
        private readonly ICancellationTokenTransport cancellationTokenTransport;

        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;

        public CloudFileDownloader(IS3Downloader s3Downloader, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.s3Downloader = s3Downloader;
            this.cancellationTokenTransport = cancellationTokenTransport;
        }

        public async Task<IHaveBeenDownloadedFromCloudToLocal[]?> DownloadObjectsFromS3(List<CloudFileDownloadRequest> metas)
        {
            return await s3Downloader.DownloadObjectsFromS3(metas, CancellationToken);
        }
    }
}