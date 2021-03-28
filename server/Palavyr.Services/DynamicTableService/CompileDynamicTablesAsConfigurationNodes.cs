using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Configuration.Schemas.DynamicTables;
using Palavyr.Services.DynamicTableService.Compilers;

namespace Palavyr.Services.DynamicTableService
{
    public interface ICompileDynamicTables
    {
        Task<List<NodeTypeOption>> CompileTables(
            IEnumerable<DynamicTableMeta> dynamicTableMetas,
            string accountId,
            string areaId
        );
    }

    public class CompileDynamicTablesAsConfigurationNodes : ICompileDynamicTables
    {
        private ILogger<CompileDynamicTablesAsConfigurationNodes> logger;
        private readonly SelectOneFlatCompiler selectOnFlatCompiler;
        private readonly PercentOfThresholdCompiler percentOfThresholdCompiler;
        private readonly BasicThresholdCompiler basicThresholdCompiler;
        private readonly CategorySelectCountCompiler categorySelectCountCompiler;

        public CompileDynamicTablesAsConfigurationNodes(
            ILogger<CompileDynamicTablesAsConfigurationNodes> logger,
            SelectOneFlatCompiler selectOnFlatCompiler,
            PercentOfThresholdCompiler percentOfThresholdCompiler,
            BasicThresholdCompiler basicThresholdCompiler,
            CategorySelectCountCompiler categorySelectCountCompiler
        )
        {
            this.logger = logger;
            this.selectOnFlatCompiler = selectOnFlatCompiler;
            this.percentOfThresholdCompiler = percentOfThresholdCompiler;
            this.basicThresholdCompiler = basicThresholdCompiler;
            this.categorySelectCountCompiler = categorySelectCountCompiler;
        }

        public async Task<List<NodeTypeOption>> CompileTables(
            IEnumerable<DynamicTableMeta> dynamicTableMetas,
            string accountId,
            string areaId
        )
        {
            var nodes = new List<NodeTypeOption>() { };
            foreach (var dynamicTableMeta in dynamicTableMetas)
            {
                var tableTypes = dynamicTableMeta.RequiredNodeTypes.Split(",");
                foreach (var tableType in tableTypes)
                {
                    switch (tableType)
                    {
                        case nameof(SelectOneFlat):
                            await selectOnFlatCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                            break;
                        case nameof(PercentOfThreshold):
                            await percentOfThresholdCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                            break;
                        case nameof(BasicThreshold):
                            await basicThresholdCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                            break;
                        case nameof(CategorySelectCount):
                            await categorySelectCountCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                            break;
                        
                        // add new node types here

                        default:
                            throw new Exception("Table logic not yet implemented");
                    }
                }
            }

            return nodes;
        }
    }
}