using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.Repositories;

namespace Palavyr.Services.DynamicTableService.Compilers
{
    public class PercentOfThresholdCompiler : BaseCompiler<PercentOfThreshold>, IDynamicTablesCompiler
    {
        public PercentOfThresholdCompiler(IGenericDynamicTableRepository<PercentOfThreshold> genericDynamicTablesRepository) : base(genericDynamicTablesRepository)
        {
        }
    }
}