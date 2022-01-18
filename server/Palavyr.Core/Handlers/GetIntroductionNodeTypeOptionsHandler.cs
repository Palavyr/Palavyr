using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Handlers
{
    public class GetIntroductionNodeTypeOptionsHandler : IRequestHandler<GetIntroductionNodeTypeOptionsRequest, GetIntroductionNodeTypeOptionsResponse>
    {
        public async Task<GetIntroductionNodeTypeOptionsResponse> Handle(GetIntroductionNodeTypeOptionsRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var introOptionList = DefaultNodeTypeOptions.IntroNodeOptionList;
            return new GetIntroductionNodeTypeOptionsResponse(introOptionList.ToArray());
        }
    }

    public class GetIntroductionNodeTypeOptionsResponse
    {
        public GetIntroductionNodeTypeOptionsResponse(NodeTypeOption[] response) => Response = response;
        public NodeTypeOption[] Response { get; set; }
    }

    public class GetIntroductionNodeTypeOptionsRequest : IRequest<GetIntroductionNodeTypeOptionsResponse>
    {
        public GetIntroductionNodeTypeOptionsRequest(string introId)
        {
            IntroId = introId;
        }

        public string IntroId { get; set; }
    }
}