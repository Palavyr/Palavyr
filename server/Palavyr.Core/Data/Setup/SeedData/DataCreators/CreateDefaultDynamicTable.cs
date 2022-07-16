using System.Collections.Generic;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.DynamicTables;

namespace Palavyr.Core.Data.Setup.SeedData.DataCreators
{
    public static class CreateDefaultDynamicTable
    {
        public static List<SimpleSelectTableRow> CreateDefaultTable(string tableTag, string accountId, string areaIdentifier, string tableId)
        {
            var selectOneFlatsDefaultData = new List<SimpleSelectTableRow>
            {
                SimpleSelectTableRow.CreateNew(
                    accountId, areaIdentifier, "Ruby", 750.00, 1200.00,
                    true, tableId, 0),
                SimpleSelectTableRow.CreateNew(
                    accountId, areaIdentifier, "Black and Tan", 500.00,
                    750.00, false, tableId, 1),
                SimpleSelectTableRow.CreateNew(
                    accountId, areaIdentifier, "Blenheim", 300.00,
                    450.00, false, tableId, 2)
            };
            return selectOneFlatsDefaultData;
        }

        public static List<PricingStrategyTableMeta> CreateDefaultMeta(string tableTag, string accountId, string tableId, string areaIdentifier)
        {
            var dynamicTableMetas = new List<PricingStrategyTableMeta>
            {
                PricingStrategyTableMeta.CreateNew(
                    tableTag,
                    new SimpleSelectTableRow().GetPrettyName(),
                    new SimpleSelectTableRow().GetTableType(),
                    tableId,
                    areaIdentifier,
                    accountId,
                    UnitIds.Currency),
            };
            return dynamicTableMetas;
        }
    }
}