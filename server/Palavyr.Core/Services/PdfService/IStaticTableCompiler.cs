using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public interface IStaticTableCompiler
    {
        Task<List<Table>> CollectStaticTables(string accountId, Area areaData, CultureInfo culture, int numIndividuals, CancellationToken cancellationToken);
    }
}