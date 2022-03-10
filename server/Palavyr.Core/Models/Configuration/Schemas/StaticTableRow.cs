using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class StaticTableRow : Entity, IStaticTableRow
    {
        public int RowOrder { get; set; }
        public string Description { get; set; } = null!;
        public StaticFee Fee { get; set; } = null!;
        public bool Range { get; set; }
        public bool PerPerson { get; set; }
        public int TableOrder { get; set; }
        public string AreaIdentifier { get; set; } = null!;
        public string AccountId { get; set; } = null!;

        public StaticTableRow()
        {
        }

        public static StaticTableRow CreateStaticTableRowTemplate(int tableOrder, string areaId, string accountId)
        {
            return CreateDefaultRow(tableOrder, areaId, accountId);
        }

        public static List<StaticTableRow> CreateDefaultStaticTable(int tableOrder, string areaId, string accountId)
        {
            return new List<StaticTableRow>()
            {
                CreateDefaultRow(tableOrder, areaId, accountId)
            };
        }

        private static StaticTableRow CreateDefaultRow(int tableOrder, string areaId, string accountId)
        {
            return new StaticTableRow
            {
                RowOrder = 0,
                Description = "Default fee description",
                Fee = StaticFee.DefaultFee(accountId, areaId),
                Range = false,
                PerPerson = false,
                TableOrder = tableOrder,
                AreaIdentifier = areaId,
                AccountId = accountId
            };
        }

        public static List<StaticTableRow> BindTemplateList(List<StaticTableRow> staticTableRows, string accountId)
        {
            var boundRows = new List<StaticTableRow>() { };
            boundRows.AddRange(
                staticTableRows.Select(
                    row => new StaticTableRow()
                    {
                        RowOrder = row.RowOrder,
                        Description = row.Description,
                        Fee = StaticFee.BindTemplate(row.Fee, accountId, row.AreaIdentifier),
                        Range = row.Range,
                        PerPerson = row.PerPerson,
                        TableOrder = row.TableOrder,
                        AreaIdentifier = row.AreaIdentifier,
                        AccountId = accountId,
                    }));
            return boundRows;
        }
    }
}