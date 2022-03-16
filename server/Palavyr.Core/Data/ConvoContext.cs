using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Data
{
    public class ConvoContext : DbContext, IDataContext
    {
        public ConvoContext(DbContextOptions<ConvoContext> options) : base(options)
        {
        }

        public DbSet<ConversationHistory> ConversationHistories { get; set; }
        public DbSet<ConversationRecord> ConversationRecords { get; set; }

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