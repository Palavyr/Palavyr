using System.Collections.Generic;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public interface IResponseHtmlBuilder
    {
        public string BuildResponseHtml(
            Account account,
            Area previewData,
            CriticalResponses criticalResponses,
            List<Table> staticTables,
            List<Table> dynamicTables,
            EmailRequest emailRequest);
    }
}