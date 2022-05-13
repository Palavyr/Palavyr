using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Resources.Requests;

namespace Palavyr.Core.Mappers
{
    public class EmailRequestMapper : IMapToPreExisting<EmailRequest, ConversationRecord>
    {
        public async Task Map(EmailRequest @from, ConversationRecord to, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            to.Name = @from.Name;
            to.Email = @from.EmailAddress;
            to.PhoneNumber = @from.Phone;
        }
    }
}