using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.PdfService;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class TypeConversionExtensionMethods
    {
        public static CloudFileDownloadRequest ToS3DownloadRequestMeta(this PdfServerResponse pdfServerResponse)
        {
            return new CloudFileDownloadRequest
            {
                FileNameWithExtension = pdfServerResponse.FileNameWithExtension,
                LocationKey = pdfServerResponse.FileAsset.LocationKey
            };
        }

        public static FileLink[] ToFileLinks(this Image[] images)
        {
            var fileLinks = new List<FileLink>();
            foreach (var image in images)
            {
                var link = image.IsUrl ? FileLink.CreateUrlLink(image.RiskyName, image.Url, image.ImageId) : FileLink.CreateS3Link(image.RiskyName, image.ImageId);
                fileLinks.Add(link);
            }

            return fileLinks.ToArray();
        }

        public static FileLink[] ToFileLinks(this List<Image> images)
        {
            return ToFileLinks(images.ToArray());
        }

        public static async Task<FileLink[]> ToFileLinks(this FileAsset[] fileAssets, ILinkCreator linkCreator)
        {
            var fileLinks = await Task.WhenAll(
                fileAssets
                    .Select(
                        async asset => new FileLink
                        {
                            Link = await linkCreator.CreateLink(asset.FileId),
                            FileId = asset.FileId,
                            FileName = asset.RiskyNameWithExtension
                        }));
            return fileLinks.ToArray();
        }
    }
}