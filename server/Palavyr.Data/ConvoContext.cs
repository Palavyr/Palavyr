using Microsoft.EntityFrameworkCore;
using Palavyr.Domain.Conversation.Schemas;

namespace DashboardServer.Data
{
    public class ConvoContext : DbContext
    {
        public ConvoContext(DbContextOptions<ConvoContext> options) : base(options) { }
        public DbSet<ConversationUpdate> Conversations { get; set; }
        public DbSet<CompletedConversation> CompletedConversations { get; set; }
        
    }
}