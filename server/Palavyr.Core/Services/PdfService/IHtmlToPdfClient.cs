#nullable enable
using System.Threading.Tasks;
using Palavyr.Core.Services.PdfService.PdfServer;

namespace Palavyr.Core.Services.PdfService
{
    public interface IHtmlToPdfClient
    {
        Task<PdfServerResponse> GeneratePdfFromHtml(string htmlString, string bucket, string locationKey, string identifier, Paper paperOptions);
    }
}