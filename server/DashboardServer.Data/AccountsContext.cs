using System;
using Microsoft.EntityFrameworkCore;
using Server.Domain.AccountDB;

namespace DashboardServer.Data
{
    public class AccountsContext : DbContext
    {
        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
        { }

        public DbSet<UserAccount> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
    }
}