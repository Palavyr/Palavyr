using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIntroductionNodeTypeOptionsHandler : IRequestHandler<GetIntroductionNodeTypeOptionsRequest, GetIntroductionNodeTypeOptionsResponse>
    {
        public async Task<GetIntroductionNodeTypeOptionsResponse> Handle(GetIntroductionNodeTypeOptionsRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var introOptionList = DefaultNodeTypeOptions.IntroNodeOptionList;
            return new GetIntroductionNodeTypeOptionsResponse(introOptionList);
        }
    }

    public class GetIntroductionNodeTypeOptionsResponse
    {
        public GetIntroductionNodeTypeOptionsResponse(IEnumerable<NodeTypeOptionResource> response) => Response = response;
        public IEnumerable<NodeTypeOptionResource> Response { get; set; }
    }

    public class GetIntroductionNodeTypeOptionsRequest : IRequest<GetIntroductionNodeTypeOptionsResponse>
    {
        public const string Route = "configure-intro/node-type-options";

        public GetIntroductionNodeTypeOptionsRequest(string introId)
        {
            IntroId = introId;
        }

        public string IntroId { get; set; }
    }
}