using System.Threading;
using System.Threading.Tasks;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Sessions;

namespace IntegrationTests.DataCreators
{
    public static class BuilderStoreExtensionMethods
    {
        public static async Task CreateAndSave<TEntity>(this IntegrationTest test, TEntity entity) where TEntity : class, IEntity
        {
            var accToken = test.ResolveType<IAccountIdTransport>();
            if (!accToken.IsSet())
            {
                test.SetAccountIdTransport();
            }

            var store = test.ResolveStore<TEntity>();
            store.ResetCancellationToken(new CancellationTokenSource(test.Timeout));
            await store.Create(entity);
        }
    }
}