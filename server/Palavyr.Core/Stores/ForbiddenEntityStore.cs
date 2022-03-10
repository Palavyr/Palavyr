using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
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

        private Type[] accountContextTypes = new[] // separated out because of a poor decision I made early on. All new tables will go into the configuration context
        {
            typeof(Account),
            typeof(EmailVerification),
            typeof(Session),
            typeof(StripeWebhookReceivedRecord),
            typeof(Subscription)
        };

        private Type[] convoTypes = new[]
        {
            typeof(ConversationHistory),
            typeof(ConversationRecord)
        };

        private readonly IUnitOfWorkContextProvider contextProvider;
        public readonly IAccountIdTransport AccountIdTransport;
        public readonly ICancellationTokenTransport CancellationTokenTransport;


        public ForbiddenEntityStore(IUnitOfWorkContextProvider contextProvider, IAccountIdTransport accountIdTransport, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.contextProvider = contextProvider;
            this.AccountIdTransport = accountIdTransport;
            this.CancellationTokenTransport = cancellationTokenTransport;
        }

        private DbContext ChooseContext()
        {
            if (accountContextTypes.Contains(typeof(TEntity)))
            {
                return contextProvider.AccountsContext();
            }
            else if (convoTypes.Contains(typeof(TEntity)))
            {
                return contextProvider.ConvoContext();
            }
            else
            {
                return contextProvider.ConfigurationContext();
            }
        }

        public async Task DANGER____DELETE_ALL_DEEP___DANGER()
        {
            await Task.CompletedTask;
            var specificContext = ChooseContext();
            var entitiesDeep = specificContext
                .Set<TEntity>()
                .Include(specificContext.GetIncludePaths(typeof(TEntity)));

            specificContext.RemoveRange(entitiesDeep);
        }
    }
}