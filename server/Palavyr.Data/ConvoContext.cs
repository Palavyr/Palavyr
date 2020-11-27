using Microsoft.EntityFrameworkCore;
using Server.Domain.Conversation;

namespace DashboardServer.Data
{
    public class ConvoContext : DbContext
    {
        public ConvoContext(DbContextOptions<ConvoContext> options) : base(options) { }
        public DbSet<ConversationUpdate> Conversations { get; set; }
        public DbSet<CompletedConversation> CompletedConversations { get; set; }
        
    }
}