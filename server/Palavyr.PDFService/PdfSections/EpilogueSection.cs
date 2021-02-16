using System.Text;

namespace PDFService.PdfSections
{
    public static class EpilogueSection
    {
        
        private static string CreateEpilogue(string epilogue)
        {
            var paragraphs = epilogue.Split("\n");
            var builder = new StringBuilder();

            builder.Append(
                $@"<section id='EPILOGUE' style='padding-left: .5in; padding-right: .5in; text-align:justify;margin-bottom: 10mm;'>");
            foreach (var para in paragraphs)
            {
                builder.Append($@"<p>{para}</p>");
            }
            builder.Append($@"</section>");
            return builder.ToString();
        }

        public static string GetEpilogue(string epilogueText)
        {
            return CreateEpilogue(epilogueText);
        }
    }
}