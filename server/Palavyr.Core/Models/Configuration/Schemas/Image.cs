#nullable enable
using System.IO;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class Image : IAccount, IId
    {
        public int? Id { get; set; }
        public string ImageId { get; set; } = null!;
        public string? SafeName { get; set; }
        public string? RiskyName { get; set; }
        public string AccountId { get; set; } = null!;
        public string? S3Key { get; set; }
        public string? Url { get; set; }
        public bool IsUrl { get; set; }

        public Image()
        {
        }

        private Image(string newImageId, string safeName, string riskyName, string accountId, string s3Key)
        {
            SafeName = safeName;
            RiskyName = riskyName;
            AccountId = accountId;
            ImageId = newImageId;
            S3Key = s3Key;
            IsUrl = false;
        }

        private Image(string imageId, string accountId, string url)
        {
            ImageId = imageId;
            AccountId = accountId;
            Url = url;
            IsUrl = true;
        }

        public static Image CreateImageRecord(string riskyName, IS3KeyResolver resolver, string accountId)
        {
            var newImageId = GuidUtils.CreateNewId();
            var extension = Path.GetExtension(riskyName);
            var safeName = string.Join("", newImageId, extension.ToLowerInvariant());
            var s3Key = resolver.ResolveImageKey(accountId, safeName);
            return new Image(newImageId, safeName, riskyName, accountId, s3Key);
        }

        public static Image CreateImageUrlRecord(string url, string accountId)
        {
            var newImageId = GuidUtils.CreateNewId();
            return new Image(newImageId, accountId, url);
        }
    }
}