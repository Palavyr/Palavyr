using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Services.PdfService
{
    public class PdfServerResponse
    {
        public FileAsset FileAsset { get; set; }
        public string FileNameWithExtension { get; set; }
        public string FileStem { get; set; }
    }
}