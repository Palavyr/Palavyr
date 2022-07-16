#nullable disable

using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class StaticTableRow : Entity, IStaticTableRow, IHaveAccountId
    {
        public int RowOrder { get; set; }
        public string Description { get; set; } = null!;
        public StaticFee Fee { get; set; } = null!;
        public bool Range { get; set; }
        public bool PerPerson { get; set; }
        public int TableOrder { get; set; }
        public string IntentId { get; set; } = null!;
        public string AccountId { get; set; } = null!;

        public StaticTableRow()
        {
        }

        public static StaticTableRow CreateStaticTableRowTemplate(int tableOrder, string intentId, string accountId)
        {
            return CreateDefaultRow(tableOrder, intentId, accountId);
        }

        public static List<StaticTableRow> CreateDefaultStaticTable(int tableOrder, string intentId, string accountId)
        {
            return new List<StaticTableRow>()
            {
                CreateDefaultRow(tableOrder, intentId, accountId)
            };
        }

        private static StaticTableRow CreateDefaultRow(int tableOrder, string intentId, string accountId)
        {
            return new StaticTableRow
            {
                RowOrder = 0,
                Description = "Default fee description",
                Fee = StaticFee.DefaultFee(accountId, intentId),
                Range = false,
                PerPerson = false,
                TableOrder = tableOrder,
                IntentId = intentId,
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
                        Fee = StaticFee.BindTemplate(row.Fee, accountId, row.IntentId),
                        Range = row.Range,
                        PerPerson = row.PerPerson,
                        TableOrder = row.TableOrder,
                        IntentId = row.IntentId,
                        AccountId = accountId,
                    }));
            return boundRows;
        }
    }
}