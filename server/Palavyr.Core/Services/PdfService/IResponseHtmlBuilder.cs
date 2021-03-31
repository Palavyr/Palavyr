using System.Collections.Generic;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public interface IResponseHtmlBuilder
    {
        string BuildResponseHtml(Account account, Area previewData, CriticalResponses response, List<Table> staticTables, List<Table> dynamicTables);
    }
}