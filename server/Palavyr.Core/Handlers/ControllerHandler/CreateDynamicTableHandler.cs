﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateDynamicTableHandler : IRequestHandler<CreateDynamicTableRequest, CreateDynamicTableResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<CreateDynamicTableHandler> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public CreateDynamicTableHandler(IConfigurationRepository configurationRepository, ILogger<CreateDynamicTableHandler> logger, IAccountIdTransport accountIdTransport)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<CreateDynamicTableResponse> Handle(CreateDynamicTableRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaComplete(request.IntentId);

            var dynamicTables = area.DynamicTableMetas.ToList();

            var tableId = Guid.NewGuid().ToString();
            var tableTag = "Default-" + StaticGuidUtils.CreatePseudoRandomString(5);

            var newTableMeta = DynamicTableMeta.CreateNew(
                tableTag,
                DynamicTableTypes.DefaultTable.PrettyName,
                DynamicTableTypes.DefaultTable.TableType,
                tableId,
                request.IntentId,
                accountIdTransport.AccountId,
                UnitIds.Currency);

            dynamicTables.Add(newTableMeta);
            area.DynamicTableMetas = dynamicTables;

            await configurationRepository.SetDefaultDynamicTable(request.IntentId, tableId);
            await configurationRepository.CommitChangesAsync();

            return new CreateDynamicTableResponse(newTableMeta);
        }
    }

    public class CreateDynamicTableResponse
    {
        public CreateDynamicTableResponse(DynamicTableMeta response) => Response = response;
        public DynamicTableMeta Response { get; set; }
    }

    public class CreateDynamicTableRequest : IRequest<CreateDynamicTableResponse>
    {
        public CreateDynamicTableRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}