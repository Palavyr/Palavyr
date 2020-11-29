using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Constant;

namespace Palavyr.API.controllers.Conversation
{
    [Route("api")]
    public class GetIsTerminalTypeController : ControllerBase
    {
        private ILogger<GetIsTerminalTypeController> logger;

        public GetIsTerminalTypeController(ILogger<GetIsTerminalTypeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("configure-conversations/check-terminal/{nodeType}")]
        public bool Get(string nodeType)
        {
            foreach (var defaultNodeType in DefaultNodeTypeOptions.DefaultNodeTypeOptionsList)
            {
                if (nodeType == defaultNodeType.Value)
                {
                    return defaultNodeType.IsTerminalType;
                }
            }
            throw new Exception("DefaultNodeType not found.");
        }
    }
}