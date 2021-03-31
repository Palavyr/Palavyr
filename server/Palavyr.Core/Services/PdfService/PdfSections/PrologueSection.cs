using System.Text;

namespace Palavyr.Core.Services.PdfService.PdfSections
{
    public static class PrologueSection
    {
        
        private static string CreatePrologue(string prologueText)
        {
            var paragraphs = prologueText.Split("\n");
            var builder = new StringBuilder();
            
            builder.Append(
                $@"<section id='PROLOGUE' style='padding-left: .5in; padding-right: .5in; text-align:justify;margin-bottom: 10mm;'>");
            foreach (var para in paragraphs)
            {
                builder.Append($@"<p>{para}</p>");
            }
            builder.Append($@"</section>");
            return builder.ToString();
        }

        public static string GetPrologue(string prologueText)
        {
            return CreatePrologue(prologueText);
        }
        
    }
}