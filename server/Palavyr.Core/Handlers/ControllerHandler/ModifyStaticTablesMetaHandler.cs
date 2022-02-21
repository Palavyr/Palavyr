using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyStaticTablesMetaHandler : IRequestHandler<ModifyStaticTablesMetaRequest, ModifyStaticTablesMetaResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<ModifyStaticTablesMetaHandler> logger;
        private readonly IHoldAnAccountId accountIdHolder;

        public ModifyStaticTablesMetaHandler(
            IConfigurationRepository configurationRepository,
            ILogger<ModifyStaticTablesMetaHandler> logger,
            IHoldAnAccountId accountIdHolder)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
            this.accountIdHolder = accountIdHolder;
        }

        public async Task<ModifyStaticTablesMetaResponse> Handle(ModifyStaticTablesMetaRequest request, CancellationToken cancellationToken)
        {
            var metasToDelete = await configurationRepository.GetStaticTables(request.IntentId);
            await configurationRepository.RemoveStaticTables(metasToDelete);

            var clearedMetas = StaticTablesMeta.BindTemplateList(request.StaticTableMetaUpdate, accountIdHolder.AccountId);
            var area = await configurationRepository.GetAreaById(request.IntentId);
            area.StaticTablesMetas = clearedMetas;

            await configurationRepository.CommitChangesAsync();

            var tables = await configurationRepository.GetStaticTables(request.IntentId);
            return new ModifyStaticTablesMetaResponse(tables);
        }
    }

    public class ModifyStaticTablesMetaResponse
    {
        public ModifyStaticTablesMetaResponse(List<StaticTablesMeta> response) => Response = response;
        public List<StaticTablesMeta> Response { get; set; }
    }

    public class ModifyStaticTablesMetaRequest : IRequest<ModifyStaticTablesMetaResponse>
    {
        public List<StaticTablesMeta> StaticTableMetaUpdate { get; set; }
        public string IntentId { get; set; }
    }
}