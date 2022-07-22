using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.Deletion
{
    public interface IIntentDeleter
    {
        Task DeleteIntent(string intentId, CancellationToken cancellationToken);
    }

    public class IntentDeleter : IIntentDeleter
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly ILogger<IIntentDeleter> logger;

        public IntentDeleter(
            IEntityStore<Intent> intentStore,
            ILogger<IntentDeleter> logger
        )
        {
            this.intentStore = intentStore;
            this.logger = logger;
        }

        public async Task DeleteIntent(string intentId, CancellationToken cancellationToken)
        {
            var completeIntent = await intentStore.GetIntentComplete(intentId);
            await intentStore.Delete(completeIntent);
        }
    }
}