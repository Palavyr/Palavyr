using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.Repositories;

namespace Palavyr.Services.DynamicTableService
{
    public abstract class BaseCompiler<TEntity> where TEntity : class, IDynamicTable<TEntity>
    {
        private readonly IGenericDynamicTableRepository<TEntity> genericDynamicTablesRepository;

        public BaseCompiler(IGenericDynamicTableRepository<TEntity> genericDynamicTablesRepository)
        {
            this.genericDynamicTablesRepository = genericDynamicTablesRepository;
        }

        protected async Task<List<TEntity>> GetTableRows(DynamicTableMeta dynamicTableMeta)
        {
            var (accountId, areaId, tableId) = dynamicTableMeta;
            var rows = await genericDynamicTablesRepository.GetAllRows(accountId, areaId, tableId);
            return rows;
        }

        public virtual async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            var nodeTypeOption = NodeTypeOption.Create(
                dynamicTableMeta.MakeUniqueIdentifier(),
                dynamicTableMeta.ConvertToPrettyName(),
                new List<string>() {"Continue"},
                new List<string>() { },
                true,
                false,
                false,
                NodeTypeOption.CustomTables
            );
            nodes.AddAdditionalNode(nodeTypeOption);
        }
    }
}