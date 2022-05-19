using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Models.Nodes
{
    public interface IRequiredNodeCalculator
    {
        Task<IEnumerable<NodeTypeOptionResource>> FindRequiredNodes(Area area);
    }

    public class RequiredNodeCalculator : IRequiredNodeCalculator
    {
        private readonly IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever;

        public RequiredNodeCalculator(IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever)
        {
            this.pricingStrategyTableCompilerRetriever = pricingStrategyTableCompilerRetriever;
        }

        bool CheckForPerIndividual(Area area)
        {
            return area
                .StaticTablesMetas
                .Select(x => x.PerPersonInputRequired)
                .Any(p => p);
        }

        public async Task<IEnumerable<NodeTypeOptionResource>> FindRequiredNodes(Area area)
        {
            var allRequiredNodes = new List<NodeTypeOptionResource>();

            if (CheckForPerIndividual(area))
            {
                allRequiredNodes.Add(DefaultNodeTypeOptions.CreateTakeNumberIndividuals());
            }

            foreach (var dynamicTableMeta in area.DynamicTableMetas)
            {
                var compiler = pricingStrategyTableCompilerRetriever.RetrieveCompiler(dynamicTableMeta.TableType);
                await compiler.CompileToConfigurationNodes(dynamicTableMeta, allRequiredNodes);
            }

            return allRequiredNodes;
        }
    }
}