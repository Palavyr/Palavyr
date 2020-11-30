using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers.Response.DynamicTables;
using Palavyr.API.Utils;
using Server.Domain.Configuration.Constant;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Services.DynamicTableService
{
    public interface ICompileDynamicTables
    {
        Task<List<NodeTypeOption>> CompileTables(
            DynamicTableMeta[] dynamicTableMetas,
            string accountId,
            string areaId
        );
    }

    public class CompileDynamicTables : ICompileDynamicTables
    {
        private object logger;
        private DashContext dashContext;

        public CompileDynamicTables(
            ILogger<CompileDynamicTables> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        public async Task<List<NodeTypeOption>> CompileTables(
            DynamicTableMeta[] dynamicTableMetas,
            string accountId,
            string areaId
        )
        {
            var dynamicTableData = new List<NodeTypeOption>() { };
            foreach (var dynamicTableMeta in dynamicTableMetas)
            {
                switch (dynamicTableMeta.TableType)
                {
                    case ("SelectOneFlat"):

                        var selectOneFlatRows = await dashContext.SelectOneFlats
                            .Where(
                                row => row.AccountId == accountId
                                       && row.AreaIdentifier == areaId
                                       && row.TableId == dynamicTableMeta.TableId)
                            .ToListAsync();
                        var valueOptions = selectOneFlatRows.Select(x => x.Option).ToList();

                        var nodeTypeOption = NodeTypeOption.Create(
                            dynamicTableMeta.MakeUniqueIdentifier(),
                            TreeUtils.TransformRequiredNodeTypeToPrettyName(dynamicTableMeta),
                            dynamicTableMeta.ValuesAsPaths ? valueOptions : new List<string>() {"Continue"},
                            valueOptions,
                            true,
                            false
                        );
                        dynamicTableData.AddAdditionalNode(nodeTypeOption);
                        break;
                    
                    default:
                        throw new Exception("Table logic not yet implemented");
                }
            }
            return dynamicTableData;
        }
    }
}