using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Response
{

    public class GetAvailableSubstitutionVariablesController : PalavyrBaseController
    {
        public GetAvailableSubstitutionVariablesController()
        {
        }

        [HttpGet("email/variables")]
        public List<ResponseVariable> Get()
        {
            return ResponseVariableDefinition.GetAvailableVariables();
        }
    }


}