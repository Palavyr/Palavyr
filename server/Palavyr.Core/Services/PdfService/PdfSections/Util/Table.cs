using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Palavyr.Core.Services.PdfService.PdfSections.Util
{
    public class Table
    {
        private string Caption { get; set; }
        private List<TableRow> Rows { get; set; } = new List<TableRow>();
        private TableRow TotalsRow { get; set; }
        public CultureInfo Culture { get; set; }
        public bool IncludeTotals { get; set; }

        public Table(string caption, List<TableRow> rows, CultureInfo culture, bool includeTotals = true)
        {
            Caption = caption;
            Rows = rows;
            Culture = culture;
            TotalsRow = SumTableRows(rows, culture);
            IncludeTotals = includeTotals;
        }

        public int Length => Rows.Count;

        private TableRow SumTableRows(List<TableRow> rows, CultureInfo culture)
        {
            var useRange = rows.Select(row => row.Range).Any(r => r);
            var usePerPerson = rows.Select(row => row.PerIndividual).Any(r => r == "Per Person"); // TODO: This should be a bool, but row holds it as parsed string

            var minTotal = 0.00;
            var maxTotal = 0.00;
            foreach (var row in rows)
            {
                if (row.Range)
                {
                    maxTotal += row.Max;
                }
                else
                {
                    maxTotal += row.Min;
                }

                minTotal += row.Min;
            }

            var perPersonMessage = usePerPerson ? " (for single individual)" : "";
            var totalsRow = new TableRow($"Total{perPersonMessage}", minTotal, maxTotal, false, culture, useRange);
            return totalsRow;
        }


        public string GenerateTableHtml()
        {
            var rowList = Rows.Select(x => x).ToList();
            if (IncludeTotals)
            {
                rowList.Add(TotalsRow);
            }

            var builder = new StringBuilder();
            builder.Append($@"<div style='margin-bottom: 10mm'>"); // TODO this goes with each table
            builder.Append(
                $@"<table style='table-layout:auto; width: 100%; border-collapse: collapse; border: 2px solid gray; '>");
            builder.Append($@"<caption>{Caption}</caption>");

            builder.Append($@"<thead id='STANDARDTABLEHEAD'>");
            builder.Append($@"<tr style='text-align: left;'>");
            builder.Append($@"<th style='padding: 5mm;'>Service Item</th>");
            builder.Append($@"<th style='padding: 5mm'; text-align: center;'>Cost</th>");
            builder.Append($@"<th style='padding: 5mm';></th>");
            builder.Append($@"</tr>");
            builder.Append($@"</thead>");

            builder.Append($@"<tbody>");
            for (var rowIndex = 0; rowIndex < rowList.Count; rowIndex++)
            {
                var color = rowIndex == rowList.Count - 1 ? "lightgray" : "none";
                builder.Append($@"<tr style='background-color: {color};'>");

                var row = rowList[rowIndex];
                var columns = new List<string>()
                {
                    row.Description, row.RowValue, row.PerIndividual
                };

                foreach (var col in columns)
                {
                    builder.Append($@"<td style='padding: 2mm' scope='col'>{col}</td>");
                }

                builder.Append($@"</tr>");
            }

            builder.Append(@"</tbody></table></div>");
            return builder.ToString();
        }

        public static Table MergeTables(List<Table> tables, CultureInfo culture, string newDescription = null)
        {
            var rows = new List<TableRow>() { };
            foreach (var table in tables)
            {
                rows.AddRange(table.Rows);
            }

            // tables could be length zero
            var firstTableDescription = newDescription ?? ((tables.Count > 0) ? tables[0].Caption : "");
            return new Table(firstTableDescription, rows, culture, false);
        }
    }
}