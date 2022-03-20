using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public interface IStaticTableCompiler
    {
        Task<List<Table>> CollectStaticTables(string intentId, CultureInfo culture, int numIndividuals);
    }
}