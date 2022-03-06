using System.Threading.Tasks;
using Palavyr.Core.Data;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories
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
            await dashContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);
            await accountsContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);
            await accountsContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);

            await dashContext.DisposeAsync();
            await accountsContext.DisposeAsync();
            await convoContext.DisposeAsync();
        }
    }
}