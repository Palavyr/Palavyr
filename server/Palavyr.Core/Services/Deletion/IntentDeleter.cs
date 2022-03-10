using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Sessions;
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
        private readonly DashContext dashContext;
        private readonly ILogger<IAreaDeleter> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public AreaDeleter(
            EntityStore<Area> intentStore,
            DashContext dashContext,
            ILogger<IAreaDeleter> logger,
            IAccountIdTransport accountIdTransport
        )
        {
            this.intentStore = intentStore;
            this.dashContext = dashContext;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task DeleteArea(string intentId, CancellationToken cancellationToken)
        {
            var completeIntent = await intentStore.GetIntentComplete(intentId);
            await intentStore.Delete(completeIntent);
        }
    }
}