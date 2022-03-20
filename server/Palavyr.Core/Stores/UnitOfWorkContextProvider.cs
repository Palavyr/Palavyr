using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Stores
{
    public class UnitOfWorkContextProvider // this must be registered as instance per lifetime scope as IUnitOfWorkContextProvider
        : IUnitOfWorkContextProvider
    {
        private readonly DashContext dashContext;
        private readonly AccountsContext accountsContext;
        private readonly ConvoContext convoContext;
        private readonly ICancellationTokenTransport cancellationTokenTransport;

        public UnitOfWorkContextProvider(DashContext dashContext, AccountsContext accountsContext, ConvoContext convoContext, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.convoContext = convoContext;
            this.cancellationTokenTransport = cancellationTokenTransport;
        }

        public DashContext ConfigurationContext()
        {
            return dashContext;
        }

        public AccountsContext AccountsContext()
        {
            return accountsContext;
        }

        public ConvoContext ConvoContext()
        {
            return convoContext;
        }

        public async Task CloseUnitOfWork()
        {
            var token = GetOrCreateCancellationToken();
            await dashContext.SaveChangesAsync(token);
            await accountsContext.SaveChangesAsync(token);
            await convoContext.SaveChangesAsync(token);
            await DisposeContexts();
        }

        public async Task DisposeContexts()
        {
            await dashContext.DisposeAsync();
            await accountsContext.DisposeAsync();
            await convoContext.DisposeAsync();
        }

        public async Task DangerousCommitAllContexts()
        {
            var token = GetOrCreateCancellationToken();
            await dashContext.SaveChangesAsync(token);
            await accountsContext.SaveChangesAsync(token);
            await convoContext.SaveChangesAsync(token);
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