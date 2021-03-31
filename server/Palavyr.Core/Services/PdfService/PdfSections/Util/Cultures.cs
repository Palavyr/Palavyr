using System.Collections.Generic;

namespace Palavyr.Core.Services.PdfService.PdfSections.Util
{
    public class Cultures
    {
        public string Culture { get; set; }

        public List<string> AvailableCultures { get; } = new List<string>()
        {
            "en-US", 
            "es-US", 
            "en-AU",
            "en-CA",
        };
    }
}