using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Handlers
{
    public class GetAvailableSubstitutionVariablesHandler : IRequestHandler<GetAvailableSubstitutionVariablesRequest, GetAvailableSubstitutionVariablesResponse>
    {
        public GetAvailableSubstitutionVariablesHandler()
        {
        }

        public async Task<GetAvailableSubstitutionVariablesResponse> Handle(GetAvailableSubstitutionVariablesRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new GetAvailableSubstitutionVariablesResponse(ResponseVariableDefinition.GetAvailableVariables());
        }
    }

    public class GetAvailableSubstitutionVariablesResponse
    {
        public GetAvailableSubstitutionVariablesResponse(List<ResponseVariable> response) => Response = response;
        public List<ResponseVariable> Response { get; set; }
    }

    public class GetAvailableSubstitutionVariablesRequest : IRequest<GetAvailableSubstitutionVariablesResponse>
    {
    }
}