using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Data
{
    public class ConvoContext : DbContext
    {
        public ConvoContext(DbContextOptions<ConvoContext> options) : base(options)
        {
        }

        public DbSet<ConversationHistory> ConversationHistories { get; set; }
        public DbSet<ConversationRecord> ConversationRecords { get; set; }
    }
}