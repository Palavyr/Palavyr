using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Services.PdfService
{
    public interface IPreviewResponseGenerator
    {
        Task<FileAsset> CreatePdfResponsePreviewAsync(string areaId, CultureInfo culture);
    }
}