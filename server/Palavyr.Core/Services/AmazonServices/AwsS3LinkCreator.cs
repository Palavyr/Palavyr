﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.AmazonServices
{
    public interface ILinkCreator
    {
        Task<string> CreateLink(string fileAssetId);
        string CreateLink(FileAsset fileAsset);
        Task<IEnumerable<string>> CreateManyLinks(IEnumerable<string> fileAssetIds);
    }


    public class AwsS3LinkCreator : ILinkCreator
    {
        private readonly IS3PreSignedUrlCreator linkCreator;
        private readonly IEntityStore<FileAsset> fileAssetStore;


        public AwsS3LinkCreator(IS3PreSignedUrlCreator s3PreSignedUrlCreator, IEntityStore<FileAsset> fileAssetStore)
        {
            this.linkCreator = s3PreSignedUrlCreator;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<string?> CreateLink(string fileAssetId)
        {
            var fileAsset = await fileAssetStore.GetOrNull(fileAssetId, s => s.FileId);
            if (fileAsset is null) return null;
            var link = linkCreator.GenericCreatePreSignedUrl(fileAsset.LocationKey);
            return link;
        }

        public string CreateLink(FileAsset fileAsset)
        {
            var link = linkCreator.GenericCreatePreSignedUrl(fileAsset.LocationKey);
            return link;
        }

        public async Task<IEnumerable<string>> CreateManyLinks(IEnumerable<string> fileAssetIds)
        {
            var fileAssets = await fileAssetStore.GetMany(fileAssetIds.ToArray(), s => s.FileId);
            var links = fileAssets.Select(asset => linkCreator.GenericCreatePreSignedUrl(asset.LocationKey));
            return links;
        }
    }
}