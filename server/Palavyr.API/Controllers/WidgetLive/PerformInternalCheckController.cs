using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.DynamicTableService;


namespace Palavyr.API.Controllers.WidgetLive
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class PerformInternalCheckController : PalavyrBaseController
    {
        private readonly ILogger<PerformInternalCheckController> logger;
        private readonly DynamicResponseComponentExtractor dynamicResponseComponentExtractor;

        public PerformInternalCheckController(
            ILogger<PerformInternalCheckController> logger,
            DynamicResponseComponentExtractor dynamicResponseComponentExtractor
            )
        {
            this.logger = logger;
            this.dynamicResponseComponentExtractor = dynamicResponseComponentExtractor;
        }
        
        [HttpPost("widget/internal-check")]
        public async Task<bool> FetchPreferences([FromBody] PreformInternalCheckControllerRequestBody internalCheckComponents)
        {
            var dynamicResponseComponents = dynamicResponseComponentExtractor.ExtractDynamicTableComponents(internalCheckComponents.CurrentDynamicResponseState);
            var result = await dynamicResponseComponents.Compiler.PerformInternalCheck(
                internalCheckComponents.Node,
                internalCheckComponents.Response,
                dynamicResponseComponents
            );
            return result;
        }
    }
    
    public class PreformInternalCheckControllerRequestBody
    {
        public ConversationNode Node { get; set; }
        public string Response { get; set; }
        public DynamicResponse CurrentDynamicResponseState { get; set; }
    }
}