using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.WidgetLive
{
    public interface ISendWidgetResponseEmailHandler
    {
        Task<SendEmailResultResponse> HandleSendWidgetResponseEmail(EmailRequest emailRequest, string accountId, string areaId, CancellationToken cancellationToken);
    }
}