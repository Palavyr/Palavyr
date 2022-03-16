using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Data
{
    public class AccountsContext : DbContext, IDataContext
    {
        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<StripeWebhookReceivedRecord> StripeWebhookReceivedRecords { get; set; }

        public AccountsContext()
        {
        }

        private IDbContextTransaction transaction;

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            transaction = await Database.BeginTransactionAsync(cancellationToken);
        }

        public void BeginTransaction()
        {
            transaction = Database.BeginTransaction();
        }

        public async Task FinalizeAsync(CancellationToken cancellationToken)
        {
            await transaction.CommitAsync(cancellationToken);
        }
    }
}