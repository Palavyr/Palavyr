using Microsoft.EntityFrameworkCore;
using Palavyr.Domain.Accounts.Schemas;

namespace Palavyr.Data
{
    public class AccountsContext : DbContext
    {
        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options) { }

        public DbSet<UserAccount> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Backup> Backups { get; set; }
        public DbSet<StripeWebhookRecord> StripeWebHookRecords { get; set; }

        public AccountsContext()
        {
            
        }
    }
}