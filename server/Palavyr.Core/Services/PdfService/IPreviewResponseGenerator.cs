using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Mappers;

namespace Palavyr.Core.Services.PdfService
{
    public interface IPreviewResponseGenerator
    {
        Task<FileAssetResource> CreatePdfResponsePreviewAsync(string areaId, CultureInfo culture);
    }
}