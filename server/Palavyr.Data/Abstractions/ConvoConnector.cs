using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DashboardServer.Data.Abstractions
{
    public interface IConvoConnector
    {
        Task CommitChangesAsync();
    }

    public class ConvoConnector : IConvoConnector
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<ConvoConnector> logger;

        public ConvoConnector(ConvoContext convoContext, ILogger<ConvoConnector> logger)
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