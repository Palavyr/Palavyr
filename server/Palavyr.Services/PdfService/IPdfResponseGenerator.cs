using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.PdfService.PdfSections.Util;

namespace Palavyr.Services.PdfService
{
    public interface IPdfResponseGenerator
    {
        Task<string> GeneratePdfResponseAsync(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string localWriteToPath,
            string identifier,
            string accountId,
            string areaId
        );
    }
}