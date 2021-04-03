using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.Core.Models
{
    public class RequiredNodeCalculator
    {
        private readonly DynamicTableCompilerRetriever dynamicTableCompilerRetriever;

        public RequiredNodeCalculator(DynamicTableCompilerRetriever dynamicTableCompilerRetriever)
        {
            this.dynamicTableCompilerRetriever = dynamicTableCompilerRetriever;
        }

        public bool CheckForPerIndividual(Area area)
        {
            return area
                .StaticTablesMetas
                .Select(x => x.PerPersonInputRequired)
                .Any(p => p);
        }

        public async Task<IEnumerable<NodeTypeOption>> GetRequiredNodes(Area area)
        {
            var allRequiredNodes = new List<NodeTypeOption>();

            if (CheckForPerIndividual(area))
            {
                allRequiredNodes.Add(DefaultNodeTypeOptions.CreateTakeNumberIndividuals());
            }

            foreach (var dynamicTableMeta in area.DynamicTableMetas)
            {
                var compiler = dynamicTableCompilerRetriever.RetrieveCompiler(dynamicTableMeta.TableType);
                await compiler.CompileToConfigurationNodes(dynamicTableMeta, allRequiredNodes);
            }

            return allRequiredNodes;
        }
    }
}