using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Sessions;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static class BuilderStoreExtensionMethods
    {
        public static async Task CreateAndSave<TEntity>(this BaseIntegrationFixture test, TEntity entity) where TEntity : class, IEntity
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