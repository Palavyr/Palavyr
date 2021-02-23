using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers.Response.DynamicTables;
using Palavyr.Data;
using Palavyr.Domain;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Services.DynamicTableService
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
        private ILogger<CompileDynamicTables> logger;
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
                if (dynamicTableMeta.TableType == DynamicTableTypes.CreateSelectOneFlat().TableType)
                {
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
                        true,
                        false, 
                        NodeTypeOption.CustomTables
                    );
                    
                    dynamicTableData.AddAdditionalNode(nodeTypeOption);
                }
                else if (dynamicTableMeta.TableType == DynamicTableTypes.CreatePercentOfThreshold().TableType)
                {
                    var nodeTypeOption = NodeTypeOption.Create(
                        dynamicTableMeta.MakeUniqueIdentifier(),
                        TreeUtils.TransformRequiredNodeTypeToPrettyName(dynamicTableMeta),
                        new List<string>() {"Continue"},
                        new List<string>() { },
                        true,
                        false,
                        false,
                        NodeTypeOption.CustomTables
                    );
                    dynamicTableData.AddAdditionalNode(nodeTypeOption);
                }
                else
                {
                    throw new Exception("Table logic not yet implemented");
                }
            }

            return dynamicTableData;
        }
    }
}