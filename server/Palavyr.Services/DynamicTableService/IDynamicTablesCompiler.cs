using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Services.DynamicTableService
{
    public interface IDynamicTablesCompiler
    {
        Task CompileToConfigurationNodes(
            DynamicTableMeta dynamicTableMeta,
            List<NodeTypeOption> nodes);
    }
}