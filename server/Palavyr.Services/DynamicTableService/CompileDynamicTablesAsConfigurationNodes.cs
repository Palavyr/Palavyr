using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Domain;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.DynamicTableService.Compilers;

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

    public class CompileDynamicTablesAsConfigurationNodes : ICompileDynamicTables
    {
        private ILogger<CompileDynamicTablesAsConfigurationNodes> logger;
        private DashContext dashContext;
        private readonly SelectOneFlatCompiler selectOnFlatCompiler;
        private readonly PercentOfThresholdCompiler percentOfThresholdCompiler;
        private readonly BasicThresholdCompiler basicThresholdCompiler;

        public CompileDynamicTablesAsConfigurationNodes(
            ILogger<CompileDynamicTablesAsConfigurationNodes> logger,
            DashContext dashContext,
            SelectOneFlatCompiler selectOnFlatCompiler,
            PercentOfThresholdCompiler percentOfThresholdCompiler,
            BasicThresholdCompiler basicThresholdCompiler
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
            this.selectOnFlatCompiler = selectOnFlatCompiler;
            this.percentOfThresholdCompiler = percentOfThresholdCompiler;
            this.basicThresholdCompiler = basicThresholdCompiler;
        }

        public async Task<List<NodeTypeOption>> CompileTables(
            DynamicTableMeta[] dynamicTableMetas,
            string accountId,
            string areaId
        )
        {
            var nodes = new List<NodeTypeOption>() { };
            foreach (var dynamicTableMeta in dynamicTableMetas)
            {
                var tableTypes = dynamicTableMeta.TableType.Split(",");
                
                if (dynamicTableMeta.TableType == nameof(SelectOneFlat))
                {
                    await selectOnFlatCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                }
                else if (dynamicTableMeta.TableType == DynamicTableTypes.CreatePercentOfThreshold().TableType)
                {
                    await percentOfThresholdCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                }
                else if (dynamicTableMeta.TableType == DynamicTableTypes.CreateBasicThreshold().TableType)
                {
                    await basicThresholdCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                }
                else
                {
                    throw new Exception("Table logic not yet implemented");
                }
            }

            return nodes;
        }
    }
}