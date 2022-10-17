using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Services.PdfService
{
    public interface IPreviewResponseGenerator
    {
        Task<FileAsset> CreatePdfResponsePreviewAsync(string intentId, CultureInfo culture, CancellationToken cancellationToken);
    }
}