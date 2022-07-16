using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Mappers
{
    public class EmailRequestMapper : IMapToPreExisting<EmailRequest, ConversationHistoryMeta>
    {
        public async Task Map(EmailRequest @from, ConversationHistoryMeta to, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            to.Name = @from.Name;
            to.Email = @from.EmailAddress;
            to.PhoneNumber = @from.Phone;
        }
    }
}