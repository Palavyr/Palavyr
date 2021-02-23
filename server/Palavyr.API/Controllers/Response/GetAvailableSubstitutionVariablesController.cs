using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Resources.Responses;

namespace Palavyr.API.Controllers.Response
{
    [Route("api")]
    [ApiController]
    public class GetAvailableSubstitutionVariablesController : ControllerBase
    {
        public GetAvailableSubstitutionVariablesController()
        {
        }

        [HttpGet("email/variables")]
        public List<ResponseVariable> Get([FromHeader] string accountId)
        {
            return ResponseVariableDefinition.GetAvailableVariables();
        }
    }


}