using System.Collections.Generic;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.DynamicTables;

namespace Palavyr.Core.Data.Setup.SeedData.DataCreators
{
    public static class CreateDefaultDynamicTable
    {
        public static List<CategorySelectTableRow> CreateDefaultTable(string tableTag, string accountId, string intentId, string tableId)
        {
            var selectOneFlatsDefaultData = new List<CategorySelectTableRow>
            {
                CategorySelectTableRow.CreateNew(
                    accountId, intentId, "Ruby", 750.00, 1200.00,
                    true, tableId, 0),
                CategorySelectTableRow.CreateNew(
                    accountId, intentId, "Black and Tan", 500.00,
                    750.00, false, tableId, 1),
                CategorySelectTableRow.CreateNew(
                    accountId, intentId, "Blenheim", 300.00,
                    450.00, false, tableId, 2)
            };
            return selectOneFlatsDefaultData;
        }

        public static List<PricingStrategyTableMeta> CreateDefaultMeta(string tableTag, string accountId, string tableId, string intentId)
        {
            var dynamicTableMetas = new List<PricingStrategyTableMeta>
            {
                PricingStrategyTableMeta.CreateNew(
                    tableTag,
                    new CategorySelectTableRow().GetPrettyName(),
                    new CategorySelectTableRow().GetTableType(),
                    tableId,
                    intentId,
                    accountId,
                    UnitIds.Currency),
            };
            return dynamicTableMetas;
        }
    }
}