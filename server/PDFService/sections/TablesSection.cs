using System.Collections.Generic;
using System.Text;

namespace PDFService
{
    public static class TablesSection
    {
        private static string CreateEstimateTables(List<Table> staticTables, List<Table> dynamicTables)
        {
            var builder = new StringBuilder();

            builder.Append($@"<section id='TABLES' style='padding-left: .5in; padding-right: .5in;'>");

            foreach (var table in dynamicTables)
            {
                builder.Append(table.GenerateTableHtml());
            }
            
            foreach (var table in staticTables)
            {
                builder.Append(table.GenerateTableHtml());
            }

            builder.Append($@"</section>");
            return builder.ToString();
        }

        public static string GetEstimateTables(List<Table> staticTables, List<Table> dynamicTables)
        {
            return CreateEstimateTables(staticTables, dynamicTables);
        }
    }
}