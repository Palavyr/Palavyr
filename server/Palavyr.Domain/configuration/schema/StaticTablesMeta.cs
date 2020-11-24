using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Server.Domain.Configuration.schema
{
    public class StaticTablesMeta
    {
        [Key] 
        public int? Id { get; set; }
        public int TableOrder { get; set; }
        public string Description { get; set; }
        public string AreaIdentifier { get; set; }
        public List<StaticTableRow> StaticTableRows { get; set; } = new List<StaticTableRow>();
        public string AccountId { get; set; }

        [NotMapped] private static string DefaultDescription { get; set; } = "Default Description";
        
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
                    AccountId = accountId
                }
            };
        }

        public static StaticTablesMeta CreateNewMetaTemplate(string areaId, string accountId)
        {
            return new StaticTablesMeta()
            {
                Description = DefaultDescription,
                AreaIdentifier = areaId,
                StaticTableRows = StaticTableRow.CreateDefaultStaticTable(0, areaId, accountId)
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
                StaticTableRows = StaticTableRow.BindTemplateList(meta.StaticTableRows, accountId)
            }));
            return boundMetas;
        }

    
    }
}