using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class DynamicTableMeta : ITable
    {
        [Key]
        public int? Id { get; set; }

        public string TableTag { get; set; }
        public string PrettyName { get; set; }
        public string TableType { get; set; }
        public string TableId { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public bool ValuesAsPaths { get; set; } = false; // for tables that specify various options, whether or not to use each option to create a new tree path.
        public bool UseTableTagAsResponseDescription { get; set; } = false;
        public UnitIds UnitId { get; set; }

        public static DynamicTableMeta CreateNew(
            string tableTag,
            string prettyName,
            string tableType,
            string tableId,
            string areaId,
            string accountId,
            UnitIds unitId
        )
        {
            return new DynamicTableMeta()
            {
                TableId = tableId,
                TableType = tableType,
                TableTag = tableTag,
                AreaIdentifier = areaId,
                AccountId = accountId,
                PrettyName = prettyName,
                UnitId = unitId
            };
        }

        public static List<DynamicTableMeta> CreateDefaultMetas(string areaId, string accountId)
        {
            return new List<DynamicTableMeta>()
            {
                CreateNew(
                    "default",
                    DynamicTableTypes.DefaultTable.PrettyName,
                    DynamicTableTypes.DefaultTable.TableType,
                    Guid.NewGuid().ToString(),
                    areaId,
                    accountId,
                    UnitIds.Currency)
            };
        }

        public void Deconstruct(out string areaId, out string tableId)
        {
            areaId = AreaIdentifier;
            tableId = TableId;
        }
    }
}