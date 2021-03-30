using System.Collections.Generic;
using System.Globalization;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.PdfService.PdfSections.Util;

namespace Palavyr.Services.PdfService
{
    public interface IStaticTableCompiler
    {
        List<Table> CollectStaticTables(Area areaData, CultureInfo culture, int numIndividuals);
    }
}