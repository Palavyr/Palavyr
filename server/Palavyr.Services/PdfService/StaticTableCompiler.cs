using System.Collections.Generic;
using System.Globalization;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.PdfService.PdfSections.Util;

namespace Palavyr.Services.PdfService
{
    public class StaticTableCompiler : IStaticTableCompiler
    {
        public StaticTableCompiler()
        {
        }

        public List<Table> CollectStaticTables(Area areaData, CultureInfo culture, int numIndividuals)
        {
            var tables = new List<Table>();
            var tableMetas = areaData.StaticTablesMetas;

            foreach (var meta in tableMetas)
            {
                var rows = new List<TableRow>();
                foreach (var dbRow in meta.StaticTableRows)
                {
                    var minFee = meta.PerPersonInputRequired ? dbRow.Fee.Min * numIndividuals : dbRow.Fee.Min;
                    var maxFee = meta.PerPersonInputRequired ? dbRow.Fee.Max * numIndividuals : dbRow.Fee.Max;
                    var perPerson = !meta.PerPersonInputRequired; // if we collect num individuals, then we don't want to show the text for it. 
                    var row = new TableRow(
                        dbRow.Description,
                        minFee,
                        maxFee,
                        perPerson,
                        culture,
                        dbRow.Range);
                    rows.Add(row);
                }

                var table = new Table(meta.Description, rows, culture);
                tables.Add(table);
            }

            return tables;
        }
    }
}