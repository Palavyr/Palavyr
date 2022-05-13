using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public interface IResponsePdfGenerator
    {
        Task<FileAsset> GeneratePdfResponse(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string intentId,
            bool isDemo
        );
    }
    
    public interface IResponsePdfTableCompiler
    {
        Task<List<Table>> CompileResponseTables(string intentId, EmailRequest emailRequest, CultureInfo culture, bool includeDynamicTableTotals);
    }

}