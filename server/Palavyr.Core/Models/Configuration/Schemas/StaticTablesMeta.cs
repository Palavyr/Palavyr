using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class StaticTablesMeta : Entity, IStaticTable
    {
        public int TableOrder { get; set; }
        public string Description { get; set; }
        public string AreaIdentifier { get; set; }
        public List<StaticTableRow> StaticTableRows { get; set; } = new List<StaticTableRow>();
        public string AccountId { get; set; }
        public bool PerPersonInputRequired { get; set; }
        public bool IncludeTotals { get; set; }

        [NotMapped]
        private static string DefaultDescription { get; } = "Default Description";

        public static List<StaticTablesMeta> CreateDefaultMetas(string areaId, string accountId)
        {
            return new List<StaticTablesMeta>()
            {
                new StaticTablesMeta()
                {
                    TableOrder = 0,
                    Description = DefaultDescription,
                    AreaIdentifier = areaId,
                    StaticTableRows = StaticTableRow.CreateDefaultStaticTable(0, areaId, accountId),
                    AccountId = accountId,
                    PerPersonInputRequired = false,
                    IncludeTotals = true
                }
            };
        }

        public static StaticTablesMeta CreateNewMetaTemplate(string areaId, string accountId)
        {
            return new StaticTablesMeta()
            {
                TableOrder = 0,
                Description = DefaultDescription,
                AreaIdentifier = areaId,
                StaticTableRows = StaticTableRow.CreateDefaultStaticTable(0, areaId, accountId),
                PerPersonInputRequired = false,
                IncludeTotals = true
            };
        }

        public static List<StaticTablesMeta> BindTemplateList(List<StaticTablesMeta> staticTablesMetas, string accountId)
        {
            var boundMetas = new List<StaticTablesMeta>() { };
            boundMetas.AddRange(
                staticTablesMetas.Select(
                    meta => new StaticTablesMeta()
                    {
                        TableOrder = meta.TableOrder,
                        Description = meta.Description,
                        AreaIdentifier = meta.AreaIdentifier,
                        AccountId = accountId,
                        StaticTableRows = StaticTableRow.BindTemplateList(meta.StaticTableRows, accountId),
                        PerPersonInputRequired = meta.PerPersonInputRequired,
                        IncludeTotals = meta.IncludeTotals
                    }));
            return boundMetas;
        }
    }
}