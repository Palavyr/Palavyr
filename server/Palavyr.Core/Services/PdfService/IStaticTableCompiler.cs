using System.Collections.Generic;
using System.Globalization;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public interface IStaticTableCompiler
    {
        List<Table> CollectStaticTables(Area areaData, CultureInfo culture, int numIndividuals);
    }
}