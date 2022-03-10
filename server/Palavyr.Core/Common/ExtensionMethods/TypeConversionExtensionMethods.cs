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

        public static async Task<FileLink> ToFileLink(this FileAsset fileAsset, ILinkCreator linkCreator)
        {
            return new FileLink
            {
                Link = await linkCreator.CreateLink(fileAsset.FileId),
                FileId = fileAsset.FileId,
                FileName = fileAsset.RiskyNameWithExtension
            };
        }
    }
}