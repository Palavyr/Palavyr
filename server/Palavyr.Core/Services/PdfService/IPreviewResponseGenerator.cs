using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Services.PdfService
{
    public interface IPreviewResponseGenerator
    {
        Task<FileAsset> CreatePdfResponsePreviewAsync(string areaId, CultureInfo culture);
    }
}