﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyShowDynamicTableTotalsHandler : IRequestHandler<ModifyShowDynamicTableTotalsRequest, ModifyShowDynamicTableTotalsResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifyShowDynamicTableTotalsHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyShowDynamicTableTotalsResponse> Handle(ModifyShowDynamicTableTotalsRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.IntentId);
            area.IncludeDynamicTableTotals = request.ShowDynamicTotals;
            return new ModifyShowDynamicTableTotalsResponse(area.IncludeDynamicTableTotals);
        }
    }

    public class ModifyShowDynamicTableTotalsResponse
    {
        public ModifyShowDynamicTableTotalsResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyShowDynamicTableTotalsRequest : IRequest<ModifyShowDynamicTableTotalsResponse>
    {
        public string IntentId { get; set; }
        public bool ShowDynamicTotals { get; set; }
    }
}