﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.AttachmentServices
{
    public interface IAttachmentRetriever
    {
        Task<FileLink[]> RetrieveAttachmentLinks(string account, string areaId, CancellationToken cancellationToken);
        Task<string[]> RetrieveAttachmentFiles(string account, string areaId, CancellationToken cancellationToken);
    }

    public class AttachmentRetriever : IAttachmentRetriever
    {
        private readonly DashContext dashContext;
        private readonly IConfiguration configuration;
        private readonly ILinkCreator linkCreator;
        private readonly IS3Retriever s3Retriever;

        public AttachmentRetriever(
            DashContext dashContext,
            IConfiguration configuration,
            ILinkCreator linkCreator,
            IS3Retriever s3Retriever
        )
        {
            this.dashContext = dashContext;
            this.configuration = configuration;
            this.linkCreator = linkCreator;
            this.s3Retriever = s3Retriever;
        }

        public async Task<FileLink[]> RetrieveAttachmentLinks(string account, string areaId, CancellationToken cancellationToken)
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

        public async Task<string[]> RetrieveAttachmentFiles(string account, string areaId, CancellationToken cancellationToken)
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

            // var s3FileKeys = metas.Select(x => x.S3Key).ToArray();
            var localFilePaths = await s3Retriever.DownloadObjectsFromS3(userDataBucket, metas, cancellationToken);
            if (localFilePaths == null)
            {
                throw new AmazonS3Exception("Unable to download to server!"); // TODO Tech debt - what is better pattern -- all these inner exceptions. No time right now.
            }
            else
            {
                for (var i = 0; i < metas.Count; i++)
                {
                    metas[i].SetLocalFilePath(localFilePaths[i]);
                }
            }

            return localFilePaths;
        }
    }
}