using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public interface IPdfResponseGenerator
    {
        Task<PdfServerResponse> GeneratePdfResponseAsync(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string identifier,
            string accountId,
            string areaId,
            CancellationToken cancellationToken
        );
    }
}