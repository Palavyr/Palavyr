using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Repositories;


namespace Palavyr.API.Controllers.Enquiries
{
    public class SelectAllController : PalavyrBaseController
    {
        private readonly IConvoHistoryRepository convoHistoryRepository;

        public SelectAllController(
            IConvoHistoryRepository convoHistoryRepository
            )
        {
            this.convoHistoryRepository = convoHistoryRepository;
        }

        [HttpPost("enquiries/selectall")]
        public async Task SelectAll()
        {
            var allRecords = await convoHistoryRepository.GetAllConversationRecords();
            foreach (var conversationRecord in allRecords)
            {
                conversationRecord.Seen = true;
            }

            await convoHistoryRepository.CommitChangesAsync();
        }



        [HttpPost("enquiries/unselectall")]
        public async Task UnSelectAll()
        {
            var allRecords = await convoHistoryRepository.GetAllConversationRecords();
            foreach (var conversationRecord in allRecords)
            {
                conversationRecord.Seen = false;
            }

            await convoHistoryRepository.CommitChangesAsync();
        }
    }
}