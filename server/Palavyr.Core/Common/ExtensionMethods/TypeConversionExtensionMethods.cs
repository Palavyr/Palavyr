using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class TypeConversionExtensionMethods
    {
        public static CloudFileDownloadRequest ToCloudFileDownloadRequest(this FileAsset fileAsset)
        {
            return new CloudFileDownloadRequest
            {
                FileNameWithExtension = fileAsset.RiskyNameWithExtension,
                LocationKey = fileAsset.LocationKey
            };
        }
        
        
        // public static CloudFileDownloadRequest ToS3DownloadRequestMeta(this PdfServerResponse pdfServerResponse)
        // {
        //     return new CloudFileDownloadRequest
        //     {
        //         FileNameWithExtension = pdfServerResponse.FileNameWithExtension,
        //         LocationKey = pdfServerResponse.FileAsset.LocationKey
        //     };
        // }

        // public static async Task<FileLink[]> ToFileLinks(this FileAsset[] fileAssets, ILinkCreator linkCreator)
        // {
        //     var fileLinks = await Task.WhenAll(
        //         fileAssets
        //             .Select(
        //                 async asset => new FileLink
        //                 {
        //                     Link = await linkCreator.CreateLink(asset.FileId),
        //                     FileId = asset.FileId,
        //                     FileName = asset.RiskyNameWithExtension
        //                 }));
        //     return fileLinks.ToArray();
        // }

        // public static async Task<FileLink> ToFileLink(this FileAsset fileAsset, ILinkCreator linkCreator)
        // {
        //     return new FileLink
        //     {
        //         Link = await linkCreator.CreateLink(fileAsset.FileId),
        //         FileId = fileAsset.FileId,
        //         FileName = fileAsset.RiskyNameWithExtension
        //     };
        // }
    }
}