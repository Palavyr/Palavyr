using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public interface IResponseHtmlBuilder
    {
        public Task<string> BuildResponseHtml(
            string intentId,
            CriticalResponses criticalResponses,
            List<Table> dynamicThenStaticTables,
            EmailRequest emailRequest);
    }
}