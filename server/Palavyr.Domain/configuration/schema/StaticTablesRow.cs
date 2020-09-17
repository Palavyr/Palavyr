using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Server.Domain.Configuration.schema
{
    public class StaticTableRow
    {
        [Key]
        public int Id { get; set; }
        public int RowOrder { get; set; }
        public string Description { get; set; }
        public StaticFee Fee { get; set; }
        public bool Range { get; set; }
        public bool PerPerson { get; set; }
        public int TableOrder { get; set; }
        public string AreaIdentifier { get; set; }
        public string AccountId { get; set; }

        public static List<StaticTableRow> CreateDefaultStaticTable(int tableId, string areaId, string accountId)
        {
            return new List<StaticTableRow>()
            {
                new StaticTableRow()
                {
                    RowOrder = 0,
                    Description = "Default fee description",
                    Fee = StaticFee.DefaultFee(accountId),
                    Range = false,
                    PerPerson = false,
                    TableOrder = tableId,
                    AreaIdentifier = areaId,
                    AccountId = accountId
                }
            };
        }

        public static List<StaticTableRow> BindTemplateList(List<StaticTableRow> staticTableRows, string accountId)
        {
            var boundRows = new List<StaticTableRow>() { };
            boundRows.AddRange(staticTableRows.Select(row => new StaticTableRow()
            {
                RowOrder = row.RowOrder,
                Description = row.Description,
                Fee = StaticFee.BindTemplate(row.Fee, accountId),
                Range = row.Range,
                PerPerson = row.PerPerson,
                TableOrder = row.TableOrder,
                AreaIdentifier = row.AreaIdentifier,
                AccountId = accountId
            }));
            return boundRows;
        }
    }
}