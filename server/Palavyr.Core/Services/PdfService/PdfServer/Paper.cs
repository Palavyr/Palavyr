using System;

namespace Palavyr.Core.Services.PdfService.PdfServer
{
    // orientation: 'portrait',
    // format: 'A4',
    // border: '.3in',
    // footer: {
    //     height: '14mm',
    //     contents: {
    //         default: `<span style="color: #444;">${formId}</span>`
    //     }
    // }

    [Serializable]
    public class Paper
    {
        public static Paper CreateDefault(string identifier)
        {
            return new Paper
            {
                orientation = PdfPaperDefaults.PaperOrientation,
                format = PdfPaperDefaults.PaperFormat,
                border = PdfPaperDefaults.PaperBorder,
                zoom = PdfPaperDefaults.ZoomFactor,
                zoomFactor = PdfPaperDefaults.ZoomFactor,
                dpi = PdfPaperDefaults.DPI,
                footer = new Footer
                {
                    height = PdfPaperDefaults.PaperFooterHeight,
                    Contents = new Contents
                    {
                        @default = PdfPaperDefaults.PaperFooterContentsDefault(identifier)
                    }
                }
            };
        }

        public string orientation { get; set; }
        public string format { get; set; }
        public string border { get; set; }
        public Footer footer { get; set; }
        public string zoom { get; set; }
        public string dpi { get; set; }
        public string zoomFactor { get; set; }
    }

    public class Footer
    {
        public string height { get; set; }
        public Contents Contents { get; set; }
    }

    public class Contents
    {
        public string @default { get; set; }
    }

    public static class PdfPaperDefaults
    {
        public const string PaperOrientation = "portrait";
        public const string PaperFormat = "A3";
        public const string PaperBorder = ".3in";
        public const string PaperFooterHeight = "14mm";
        public const string ZoomFactor = "2.0";

        public const string DPI = "100";

        public static readonly Func<string, string> PaperFooterContentsDefault = id => $"<span style=\"color: #444;\">${id}</span>";
    }
}