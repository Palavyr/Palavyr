using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Services.PdfService
{
    public interface IPreviewResponseGenerator
    {
        Task<FileLink> CreatePdfResponsePreviewAsync(string accountId, string areaId, CultureInfo culture, CancellationToken cancellationToken);
    }
}