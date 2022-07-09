using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.Core.Data.Setup.SeedData.DataCreators
{
    public static class CreateDefaultDynamicTable
    {
        public static List<SelectOneFlat> CreateDefaultTable(string tableTag, string accountId, string areaIdentifier, string tableId)
        {
            var selectOneFlatsDefaultData = new List<SelectOneFlat>
            {
                SelectOneFlat.CreateNew(
                    accountId, areaIdentifier, "Ruby", 750.00, 1200.00,
                    true, tableId, 0),
                SelectOneFlat.CreateNew(
                    accountId, areaIdentifier, "Black and Tan", 500.00,
                    750.00, false, tableId, 1),
                SelectOneFlat.CreateNew(
                    accountId, areaIdentifier, "Blenheim", 300.00,
                    450.00, false, tableId, 2)
            };
            return selectOneFlatsDefaultData;
        }

        public static List<DynamicTableMeta> CreateDefaultMeta(string tableTag, string accountId, string tableId, string areaIdentifier)
        {
            var dynamicTableMetas = new List<DynamicTableMeta>
            {
                DynamicTableMeta.CreateNew(
                    tableTag,
                    new SelectOneFlat().GetPrettyName(),
                    new SelectOneFlat().GetTableType(),
                    tableId,
                    areaIdentifier,
                    accountId,
                    UnitIds.Currency),
            };
            return dynamicTableMetas;
        }
    }
}