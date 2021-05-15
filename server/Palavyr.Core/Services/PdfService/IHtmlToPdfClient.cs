#nullable enable
using System.Threading.Tasks;

namespace Palavyr.Core.Services.PdfService
{
    public interface IHtmlToPdfClient
    {
        Task<PdfServerResponse> GeneratePdfFromHtmlOrNull(string htmlString, string localWriteToPath, string identifier);
    }
}