using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.Core.Models.Nodes
{
    public class RequiredNodeCalculator
    {
        private readonly IDynamicTableCompilerRetriever dynamicTableCompilerRetriever;

        public RequiredNodeCalculator(IDynamicTableCompilerRetriever dynamicTableCompilerRetriever)
        {
            this.dynamicTableCompilerRetriever = dynamicTableCompilerRetriever;
        }

        bool CheckForPerIndividual(Area area)
        {
            return area
                .StaticTablesMetas
                .Select(x => x.PerPersonInputRequired)
                .Any(p => p);
        }

        public async Task<IEnumerable<NodeTypeOption>> FindRequiredNodes(Area area)
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