﻿using System.Collections.Generic;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Accounts.Setup.SeedData.DataCreators
{
    public static class CreateDefaultDynamicTable
    {
        public static List<SelectOneFlat> CreateDefaultTable(string tableTag, string accountId, string areaIdentifier, string tableId)
        {
            var selectOneFlatsDefaultData = new List<SelectOneFlat>
            {
                SelectOneFlat.CreateNew(accountId, areaIdentifier, "Ruby", 750.00, 1200.00,
                    true, tableId),
                SelectOneFlat.CreateNew(accountId, areaIdentifier, "Black and Tan", 500.00,
                    750.00, false, tableId),
                SelectOneFlat.CreateNew(accountId, areaIdentifier, "Blenheim", 300.00,
                450.00, false, tableId)
            };
            return selectOneFlatsDefaultData;
        }

        public static List<DynamicTableMeta> CreateDefaultMeta(string tableTag, string accountId, string tableId, string areaIdentifier)
        {
            var dynamicTableMetas = new List<DynamicTableMeta>()
            {
                DynamicTableMeta.CreateNew(
                    tableTag, 
                    DynamicTableTypes.DefaultTable.PrettyName,
                    DynamicTableTypes.DefaultTable.TableType, 
                    tableId, 
                    areaIdentifier,
                    accountId),
            };
            return dynamicTableMetas;

        }
    }
}