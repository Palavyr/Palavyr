using System.Threading.Tasks;

namespace Palavyr.Core.Services.PdfService
{
    public interface IHtmlToPdfClient
    {
        Task<string> GeneratePdfFromHtml(string htmlString, string localWriteToPath, string identifier);
    }
}