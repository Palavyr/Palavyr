
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.PdfService.PdfServer;

namespace Palavyr.Core.Services.PdfService
{
    public interface IHtmlToPdfClient
    {
        Task<FileAsset> GeneratePdfFromHtml(string htmlString, string locationKey, string identifier, Paper paperOptions);
    }
}