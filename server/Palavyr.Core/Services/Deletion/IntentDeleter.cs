﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.Deletion
{
    public interface IAreaDeleter
    {
        Task DeleteArea(string areaId, CancellationToken cancellationToken);
    }

    public class AreaDeleter : IAreaDeleter
    {
        private readonly EntityStore<Area> intentStore;
        private readonly ILogger<IAreaDeleter> logger;

        public AreaDeleter(
            EntityStore<Area> intentStore,
            ILogger<IAreaDeleter> logger
        )
        {
            this.intentStore = intentStore;
            this.logger = logger;
        }

        public async Task DeleteArea(string intentId, CancellationToken cancellationToken)
        {
            var completeIntent = await intentStore.GetIntentComplete(intentId);
            await intentStore.Delete(completeIntent);
        }
    }
}