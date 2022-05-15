using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAvailableSubstitutionVariablesHandler : IRequestHandler<GetAvailableSubstitutionVariablesRequest, GetAvailableSubstitutionVariablesResponse>
    {
        private readonly IMapToNew<ResponseVariableDefinition, ResponseVariableResource> mapper;

        public GetAvailableSubstitutionVariablesHandler(IMapToNew<ResponseVariableDefinition, ResponseVariableResource> mapper)
        {
            this.mapper = mapper;
        }

        public async Task<GetAvailableSubstitutionVariablesResponse> Handle(GetAvailableSubstitutionVariablesRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var resource = await mapper.Map(new ResponseVariableDefinition());
            return new GetAvailableSubstitutionVariablesResponse(resource);
        }
    }

    public class GetAvailableSubstitutionVariablesResponse
    {
        public GetAvailableSubstitutionVariablesResponse(ResponseVariableResource resource) => Resource = resource;
        public ResponseVariableResource Resource { get; set; }
    }

    public class GetAvailableSubstitutionVariablesRequest : IRequest<GetAvailableSubstitutionVariablesResponse>
    {
    }
}