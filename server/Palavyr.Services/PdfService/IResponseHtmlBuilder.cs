using System.Collections.Generic;
using Palavyr.Domain.Accounts.Schemas;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.PdfService.PdfSections.Util;

namespace Palavyr.Services.PdfService
{
    public interface IResponseHtmlBuilder
    {
        string BuildResponseHtml(Account account, Area previewData, CriticalResponses response, List<Table> staticTables, List<Table> dynamicTables);
    }
}