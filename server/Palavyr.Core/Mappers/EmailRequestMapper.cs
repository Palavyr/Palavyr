using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Requests;

namespace Palavyr.Core.Mappers
{
    public class EmailRequestMapper : IMapToNew<EmailRequest, ConversationRecord>
    {
        public async Task<ConversationRecord> Map(EmailRequest @from)
        {
            await Task.CompletedTask;
            return new ConversationRecord
            {
                Name = @from.Name,
                Email = @from.EmailAddress,
                PhoneNumber = @from.Phone
            };
        }
    }
}