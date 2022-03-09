using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Data
{
    public class AccountsContext : DbContext
    {
        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<StripeWebhookRecord> StripeWebHookRecords { get; set; }

        public AccountsContext()
        {
            
        }
    }
}