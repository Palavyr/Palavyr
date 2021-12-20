using System.Collections.Generic;
using System.Text;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService.PdfSections
{
    public static class TablesSection
    {
        public static string GetEstimateTables(List<Table> staticTables, List<Table> dynamicTables)
        {
            return CreateEstimateTables(staticTables, dynamicTables);
        }

        private static string CreateEstimateTables(List<Table> staticTables, List<Table> dynamicTables)
        {
            var builder = new StringBuilder();

            builder.Append($@"<section id='TABLES' style='padding-left: .5in; padding-right: .5in;'>");

            foreach (var table in dynamicTables)
            {
                if (table.Length > 0)
                {
                    builder.Append(table.GenerateTableHtml());
                }
            }
            
            foreach (var table in staticTables)
            {
                if (table.Length > 0)
                {
                    builder.Append(table.GenerateTableHtml());
                }
            }

            builder.Append($@"</section>");
            return builder.ToString();
        }
    }
}