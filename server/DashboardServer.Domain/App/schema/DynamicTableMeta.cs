using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Domain.App.schema
{
    public class DynamicTableMeta
    {
        [Key] 
        public int Id { get; set; }
        public string TableTag { get; set; }
        public string PrettyName { get; set; }
        public string TableType { get; set; }
        public string TableId { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public bool ValuesAsPaths { get; set; } = false; // for tables that specify various options, whether or not to use each option to create a new tree path.
        
        public static DynamicTableMeta CreateNew(string tableName, string prettyName, string tableType, string tableId, string areaId, string accountId)
        {
            return new DynamicTableMeta()
            {
                TableId = tableId,
                TableType = tableType,
                TableTag = tableName,
                AreaIdentifier = areaId,
                AccountId = accountId,
                PrettyName = prettyName
            };
        }

        public static List<DynamicTableMeta> CreateDefaultMetas(string areaId, string accountId)
        {
            return new List<DynamicTableMeta>()
            {
                DynamicTableMeta.CreateNew("default", DynamicTableTypes.DefaultPrettyName,DynamicTableTypes.SelectOneFlat, Guid.NewGuid().ToString(), areaId, accountId)
            };
        }
    }
}
