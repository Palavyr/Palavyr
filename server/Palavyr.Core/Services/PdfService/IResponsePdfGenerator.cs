using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public interface IResponsePdfGenerator
    {
        Task<PdfServerResponse> GeneratePdfResponse(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string uniqueId,
            string intentId
        );
    }
}