using Microsoft.EntityFrameworkCore;
using Server.Domain.Accounts;

namespace DashboardServer.Data
{
    public class AccountsContext : DbContext
    {
        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options) { }

        public DbSet<UserAccount> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

    }
}