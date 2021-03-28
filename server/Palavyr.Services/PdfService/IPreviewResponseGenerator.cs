using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Domain.Resources.Responses;

namespace Palavyr.Services.PdfService
{
    public interface IPreviewResponseGenerator
    {
        Task<FileLink> CreatePdfResponsePreviewAsync(string accountId, string areaId, CultureInfo culture);
    }
}