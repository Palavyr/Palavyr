using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.Core.Services.PdfService
{
    public class PdfServerResponse : IHoldTemporaryPathDetails
    {
        public string FullPath { get; set; }
        public string TempDirectory { get; set; }
        public string FileNameWithExtension { get; set; }
        public string FileStem { get; set; }
    }
}