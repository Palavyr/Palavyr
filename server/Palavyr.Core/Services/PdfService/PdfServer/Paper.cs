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
        public string Orientation { get; set; }
        public string Format { get; set; }
        public string Border { get; set; }
        public Footer Footer { get; set; }

        public static Paper CreateDefault(string identifier)
        {
            return new Paper
            {
                Orientation = PdfPaperDefaults.PaperOrientation,
                Format = PdfPaperDefaults.PaperFormat,
                Border = PdfPaperDefaults.PaperBorder,
                Footer = new Footer
                {
                    Height = PdfPaperDefaults.PaperFooterHeight,
                    Contents = new Contents
                    {
                        Default = PdfPaperDefaults.PaperFooterContentsDefault(identifier)
                    }
                }
            };
        }
    }

    public class Footer
    {
        public string Height { get; set; }
        public Contents Contents { get; set; }
    }

    public class Contents
    {
        public string Default { get; set; }
    }

    public static class PdfPaperDefaults
    {
        public const string PaperOrientation = "portrait";
        public const string PaperFormat = "A4";
        public const string PaperBorder = ".3in";
        public const string PaperFooterHeight = "14mm";
        public static readonly Func<string, string> PaperFooterContentsDefault = id => $"<span style=\"color: #444;\">${id}</span>";
    }
}