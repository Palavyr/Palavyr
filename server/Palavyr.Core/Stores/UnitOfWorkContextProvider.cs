using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Stores
{
    // this must be registered as instance per lifetime scope as IUnitOfWorkContextProvider
    public class UnitOfWorkContextProvider : IUnitOfWorkContextProvider
    {
        private readonly AppDataContexts appDataContexts;
        private readonly ICancellationTokenTransport cancellationTokenTransport;

        public UnitOfWorkContextProvider(AppDataContexts appDataContexts, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.appDataContexts = appDataContexts;
            this.cancellationTokenTransport = cancellationTokenTransport;
        }

        public AppDataContexts AppDataContexts()
        {
            return appDataContexts;
        }

        public async Task CloseUnitOfWork()
        {
            var token = GetOrCreateCancellationToken();
            await appDataContexts.SaveChangesAsync(token);
            await DisposeContexts();
        }

        public async Task DisposeContexts()
        {
            await appDataContexts.DisposeAsync();
        }

        public async Task DangerousCommitAllContexts()
        {
            var token = GetOrCreateCancellationToken();
            await appDataContexts.SaveChangesAsync(token);
        }

        private CancellationToken GetOrCreateCancellationToken()
        {
            CancellationToken token;
            // starting to notice how this transport idea makes testing more complicated
            // tests need to write data to the DB - so we have to commit that data since we don't
            // always write data AND tests via the api. Instead we reach in and commit with this method
            // so if we apply the cancellation token via the middleware, then we can't set it in the builders...
            // but that only for tests that use the middleware! Suddenly we have two kinds of integration tests -
            // those that go through the api, and those that just test component integration.
            // That wouldn't matter if we didn't have transports defining the line between them.
            // ...But lets keep going with this experiment to see how bad things really get. 
            if (cancellationTokenTransport.IsSet())
            {
                token = cancellationTokenTransport.CancellationToken;
            }
            else
            {
                var cts = new CancellationTokenSource();
                token = cts.Token;
            }

            return token;
        }
    }
}