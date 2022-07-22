using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Models.Nodes
{
    public interface IRequiredNodeCalculator
    {
        Task<IEnumerable<NodeTypeOptionResource>> FindRequiredNodes(Intent intent);
    }

    public class RequiredNodeCalculator : IRequiredNodeCalculator
    {
        private readonly IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever;

        public RequiredNodeCalculator(IPricingStrategyTableCompilerRetriever pricingStrategyTableCompilerRetriever)
        {
            this.pricingStrategyTableCompilerRetriever = pricingStrategyTableCompilerRetriever;
        }

        bool CheckForPerIndividual(Intent intent)
        {
            return intent
                .StaticTablesMetas
                .Select(x => x.PerPersonInputRequired)
                .Any(p => p);
        }

        public async Task<IEnumerable<NodeTypeOptionResource>> FindRequiredNodes(Intent intent)
        {
            var allRequiredNodes = new List<NodeTypeOptionResource>();

            if (CheckForPerIndividual(intent))
            {
                allRequiredNodes.Add(DefaultNodeTypeOptions.CreateTakeNumberIndividuals());
            }

            foreach (var pricingStrategyTableMeta in intent.PricingStrategyTableMetas)
            {
                var compiler = pricingStrategyTableCompilerRetriever.RetrieveCompiler(pricingStrategyTableMeta.TableType);
                await compiler.CompileToConfigurationNodes(pricingStrategyTableMeta, allRequiredNodes);
            }

            return allRequiredNodes;
        }
    }
}