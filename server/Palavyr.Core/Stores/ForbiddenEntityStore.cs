using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Stores
{
    public interface IForbiddenEntityStore
    {
        Task DANGER____DELETE_ALL_DEEP___DANGER();
    }

    public class ForbiddenEntityStore<TEntity> : IForbiddenEntityStore where TEntity : class
    {
        public CancellationToken CancellationToken => CancellationTokenTransport.CancellationToken;
        public string AccountId => AccountIdTransport.AccountId;
        private readonly IUnitOfWorkContextProvider contextProvider;
        public readonly IAccountIdTransport AccountIdTransport;
        public readonly ICancellationTokenTransport CancellationTokenTransport;


        public ForbiddenEntityStore(IUnitOfWorkContextProvider contextProvider, IAccountIdTransport accountIdTransport, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.contextProvider = contextProvider;
            this.AccountIdTransport = accountIdTransport;
            this.CancellationTokenTransport = cancellationTokenTransport;
        }

        public async Task DANGER____DELETE_ALL_DEEP___DANGER()
        {
            await Task.CompletedTask;
            // var entitiesDeep = contextProvider.DbContexts()
            //     .Set<TEntity>()
            //     .Include(specificContext.GetIncludePaths(typeof(TEntity)));
            //
            // specificContext.RemoveRange(entitiesDeep);
        }
    }
}