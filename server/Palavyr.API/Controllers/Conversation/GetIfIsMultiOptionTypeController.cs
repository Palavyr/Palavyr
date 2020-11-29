using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Constant;

namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    public class GetIfIsMultiOptionTypeController : ControllerBase
    {
        private ILogger<GetIfIsMultiOptionTypeController> logger;

        public GetIfIsMultiOptionTypeController(ILogger<GetIfIsMultiOptionTypeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("configure-conversations/check-multi-option/{nodeType}")]
        public bool Get(string nodeType)
        {
            foreach (var defaultNodeType in DefaultNodeTypeOptions.DefaultNodeTypeOptionsList)
            {
                if (nodeType == defaultNodeType.Value)
                {
                    return defaultNodeType.IsMultiOptionType;
                }
            }
            throw new Exception("DefaultNodeType not found.");
        }
    }
}