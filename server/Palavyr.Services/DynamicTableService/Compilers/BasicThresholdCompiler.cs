using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.Repositories;

namespace Palavyr.Services.DynamicTableService.Compilers
{
    public class BasicThresholdCompiler : BaseCompiler<BasicThreshold>, IDynamicTablesCompiler
    {
        public BasicThresholdCompiler(IGenericDynamicTableRepository<BasicThreshold> genericDynamicTablesRepository) : base(genericDynamicTablesRepository)
        {
        }
    }
}