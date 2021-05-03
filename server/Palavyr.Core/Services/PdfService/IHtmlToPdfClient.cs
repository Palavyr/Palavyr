#nullable enable
using System.Threading.Tasks;

namespace Palavyr.Core.Services.PdfService
{
    public interface IHtmlToPdfClient
    {
        Task<string?> GeneratePdfFromHtmlOrNull(string htmlString, string localWriteToPath, string identifier);
    }
}