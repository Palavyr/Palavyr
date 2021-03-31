using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;

namespace Palavyr.Core.Repositories
{
    public class ConvoHistoryRepository : IConvoHistoryRepository
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<ConvoHistoryRepository> logger;

        public ConvoHistoryRepository(ConvoContext convoContext, ILogger<ConvoHistoryRepository> logger)
        {
            this.convoContext = convoContext;
            this.logger = logger;
        }
        
        public async Task CommitChangesAsync()
        {
            await convoContext.SaveChangesAsync();
        }
    }
}