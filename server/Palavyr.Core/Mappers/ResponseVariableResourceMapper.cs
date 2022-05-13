using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Resources;
using Palavyr.Core.Resources.Responses;

namespace Palavyr.Core.Mappers
{
    public class ResponseVariableResourceMapper : IMapToNew<ResponseVariableDefinition, ResponseVariableResource>
    {
        public async Task<ResponseVariableResource> Map(ResponseVariableDefinition @from)
        {
            await Task.CompletedTask;
            return new ResponseVariableResource
            {
                ResponseVariables = GetAvailableVariables()
            };
        }
        
        List<ResponseVariable> GetAvailableVariables()
        {
            return new List<ResponseVariable>()
            {
                new ResponseVariable(ResponseVariableDefinition.Company, ResponseVariableDefinition.CompanyVariablePattern, "Your company name provided in Settings."),
                new ResponseVariable(ResponseVariableDefinition.Name, ResponseVariableDefinition.NameVariablePattern, "The name provided by the client in the chat dialog."),
                new ResponseVariable(ResponseVariableDefinition.Logo, ResponseVariableDefinition.LogoUriVariablePattern, "The logo you provided in Settings"),
            };
        }
    }
}