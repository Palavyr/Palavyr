using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.PdfService;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class TypeConversionExtensionMethods
    {
        public static S3SDownloadRequestMeta ToS3DownloadRequestMeta(this PdfServerResponse pdfServerResponse)
        {
            return new S3SDownloadRequestMeta
            {
                FileNameWithExtension = pdfServerResponse.FileNameWithExtension,
                S3Key = pdfServerResponse.S3Key
            };
        }

        public static FileLink[] ToFileLinks(this Image[] images)
        {
            var fileLinks = new List<FileLink>();
            foreach (var image in images)
            {
                var link = image.IsUrl ? FileLink.CreateUrlLink(image.RiskyName, image.Url, image.ImageId) : FileLink.CreateS3Link(image.RiskyName,image.ImageId, image.S3Key);
                fileLinks.Add(link);
            }

            return fileLinks.ToArray();
        }

        public static FileLink[] ToFileLinks(this List<Image> images)
        {
            return ToFileLinks(images.ToArray());
        }


        public static FileLink[] ImageUrlToFileLinks(this Image image)
        {
            return new[] {image}.ImageUrlToFileLinks();
        }

        public static FileLink[] ImageUrlToFileLinks(this Image[] images)
        {
            var fileLinks = new List<FileLink>();
            foreach (var image in images)
            {
                var link = FileLink.CreateUrlLink(image.RiskyName, image.Url, image.ImageId);
                fileLinks.Add(link);
            }

            return fileLinks.ToArray();
        }
    }
}