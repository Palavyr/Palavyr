using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Data;

namespace Palavyr.Services.DatabaseService
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