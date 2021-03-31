using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class CreateDynamicTableController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<CreateDynamicTableController> logger;

        public CreateDynamicTableController(IConfigurationRepository configurationRepository, ILogger<CreateDynamicTableController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        // One controller for getting each table type. A separate call to get the type. Each table has a different
        // structure, and thus a different type. So we can't return multiple types from the same controller without
        // further generalization. This can be done later if its worth it. Adding a new controller for each type
        // isn't that big of a deal since we'll only have dozens of types probably. If we make money, then we can switch
        // to a generic pattern. Its just too complex to implement right now.

        [HttpPost("tables/dynamic/{areaId}")]
        public async Task<DynamicTableMeta> Create(
            [FromHeader] string accountId,
            [FromRoute] string areaId)
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);

            var dynamicTables = area.DynamicTableMetas.ToList();

            var tableId = Guid.NewGuid().ToString();
            var tableTag = "Default-" + GuidUtils.CreatePseudoRandomString(5);

            var newTableMeta = DynamicTableMeta.CreateNew(
                tableTag,
                DynamicTableTypes.DefaultTable.PrettyName,
                DynamicTableTypes.DefaultTable.TableType,
                tableId,
                areaId,
                accountId);

            dynamicTables.Add(newTableMeta);
            area.DynamicTableMetas = dynamicTables;

            await configurationRepository.SetDefaultDynamicTable(accountId, areaId, tableId);
            await configurationRepository.CommitChangesAsync();

            return newTableMeta;
        }
    }
}