using System;
using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;
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
                var link = image.IsUrl ? FileLink.CreateLink(image.RiskyName, image.Url, image.ImageId, true) : FileLink.CreateLink(image.RiskyName, image.S3Key, image.ImageId);
                fileLinks.Add(link);
            }

            return fileLinks.ToArray();
        }

        public static FileLink[] ToFileLinks(this List<Image> images)
        {
            return ToFileLinks(images.ToArray());
        }

        public static FileLink[] ToFileLinks(this List<Image> images, ILinkCreator linkCreator, string bucket)
        {
            return ToFileLinks(images.ToArray(), linkCreator, bucket);
        }

        public static FileLink[] ToFileLinks(this Image[] images, ILinkCreator linkCreator, string bucket)
        {
            var fileLinks = new List<FileLink>();
            foreach (var image in images)
            {
                var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(image.S3Key, bucket, DateTime.Now.AddDays(6.5));
                var link = FileLink.CreateLink(image.RiskyName, preSignedUrl, image.ImageId);
                fileLinks.Add(link);
            }

            return fileLinks.ToArray();
        }

        public static FileLink[] ToFileLinks(this Image image, ILinkCreator linkCreator, string bucket)
        {
            return new[] {image}.ToFileLinks(linkCreator, image.ImageId);
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
                var link = FileLink.CreateLink(image.RiskyName, image.Url, image.ImageId, true);
                fileLinks.Add(link);
            }

            return fileLinks.ToArray();
        }
    }
}