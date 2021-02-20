using System.Collections.Generic;

namespace Palavyr.Services.PdfService.PdfSections.Util
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