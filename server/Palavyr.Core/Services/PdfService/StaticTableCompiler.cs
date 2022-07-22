using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.AccountServices.PlanTypes;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.PdfService
{
    public class StaticTableCompiler : IStaticTableCompiler
    {
        private readonly IBusinessRules businessRules;
        private readonly IEntityStore<Intent> intentStore;

        public StaticTableCompiler(
            IBusinessRules businessRules,
            IEntityStore<Intent> intentStore)
        {
            this.businessRules = businessRules;
            this.intentStore = intentStore;
        }

        public async Task<List<Table>> CollectStaticTables(string intentId, CultureInfo culture, int numIndividuals)
        {

            var tables = new List<Table>();

            var intentComplete = await intentStore.GetIntentComplete(intentId);
            var tableMetas = intentComplete.StaticTablesMetas;

            foreach (var meta in tableMetas)
            {
                var rows = new List<TableRow>();
                foreach (var dbRow in meta.StaticTableRows)
                {
                    var minFee = meta.PerPersonInputRequired ? dbRow.Fee.Min * numIndividuals : dbRow.Fee.Min;
                    var maxFee = meta.PerPersonInputRequired ? dbRow.Fee.Max * numIndividuals : dbRow.Fee.Max;
                    var perPerson = !meta.PerPersonInputRequired && dbRow.PerPerson; // if we collect num individuals, then we don't want to show the text for it. 
                    var row = new TableRow(
                        dbRow.Description,
                        minFee,
                        maxFee,
                        perPerson,
                        culture,
                        dbRow.Range);
                    rows.Add(row);
                }

                var table = new Table(meta.Description, rows, culture, meta.IncludeTotals);
                tables.Add(table);
            }

            var numberStaticTablesAllowed = await businessRules.GetAllowedStaticTables();
            return tables.Take(numberStaticTablesAllowed).ToList();
        }
    }
}