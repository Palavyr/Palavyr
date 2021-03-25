using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Palavyr.Domain.Contracts;

namespace Palavyr.Domain.Configuration.Schemas
{
    public class StaticTablesMeta : IStaticTable
    {
        [Key] 
        public int? Id { get; set; }
        public int TableOrder { get; set; }
        public string Description { get; set; }
        public string AreaIdentifier { get; set; }
        public List<StaticTableRow> StaticTableRows { get; set; } = new List<StaticTableRow>();
        public string AccountId { get; set; }
        public bool PerPersonInputRequired { get; set; }

        [NotMapped] private static string DefaultDescription { get; } = "Default Description";
        
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
                    PerPersonInputRequired = false
                }
            };
        }

        public static StaticTablesMeta CreateNewMetaTemplate(string areaId, string accountId)
        {
            return new StaticTablesMeta()
            {
                Description = DefaultDescription,
                AreaIdentifier = areaId,
                StaticTableRows = StaticTableRow.CreateDefaultStaticTable(0, areaId, accountId),
                PerPersonInputRequired = false
            };
        }

        public static List<StaticTablesMeta> BindTemplateList(List<StaticTablesMeta> staticTablesMetas, string accountId)
        {
            var boundMetas = new List<StaticTablesMeta>() { };
            boundMetas.AddRange(staticTablesMetas.Select(meta => new StaticTablesMeta()
            {
                TableOrder = meta.TableOrder,
                Description = meta.Description,
                AreaIdentifier = meta.AreaIdentifier,
                AccountId = accountId,
                StaticTableRows = StaticTableRow.BindTemplateList(meta.StaticTableRows, accountId),
                PerPersonInputRequired = meta.PerPersonInputRequired
            }));
            return boundMetas;
        }

    
    }
}