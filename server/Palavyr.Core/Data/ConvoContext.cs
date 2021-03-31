using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Data
{
    public class ConvoContext : DbContext
    {
        public ConvoContext(DbContextOptions<ConvoContext> options) : base(options) { }
        public DbSet<ConversationUpdate> Conversations { get; set; }
        public DbSet<CompletedConversation> CompletedConversations { get; set; }
        
    }
}