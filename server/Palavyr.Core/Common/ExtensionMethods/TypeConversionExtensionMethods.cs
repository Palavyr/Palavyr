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
    }
}