using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetMissingIntroductionSequenceTreeErrorsHandler : IRequestHandler<GetMissingIntroductionSequenceTreeErrorsRequest, GetMissingIntroductionSequenceTreeErrorsResponse>
    {
        private readonly ILogger<GetMissingIntroductionSequenceTreeErrorsHandler> logger;

        public GetMissingIntroductionSequenceTreeErrorsHandler(
            ILogger<GetMissingIntroductionSequenceTreeErrorsHandler> logger
        )
        {
            this.logger = logger;
        }

        public async Task<GetMissingIntroductionSequenceTreeErrorsResponse> Handle(GetMissingIntroductionSequenceTreeErrorsRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var missingNodes = new List<string> { };
            if (!request.Transactions.Select(x => x.NodeType).Contains(DefaultNodeTypeOptions.Selection.StringName))
            {
                missingNodes.Add(DefaultNodeTypeOptions.Selection.StringName);
            }

            if (!request.Transactions.Select(x => x.NodeType).Contains(DefaultNodeTypeOptions.CollectDetails.StringName))
            {
                missingNodes.Add(DefaultNodeTypeOptions.CollectDetails.StringName);
            }

            var treeErrors = new TreeErrorsResource(missingNodes.ToArray(), new string[] { });
            return new GetMissingIntroductionSequenceTreeErrorsResponse(treeErrors);
        }
    }

    public class GetMissingIntroductionSequenceTreeErrorsResponse
    {
        public GetMissingIntroductionSequenceTreeErrorsResponse(TreeErrorsResource resource) => Resource = resource;
        public TreeErrorsResource Resource { get; set; }
    }

    public class GetMissingIntroductionSequenceTreeErrorsRequest : IRequest<GetMissingIntroductionSequenceTreeErrorsResponse>
    {
        public List<ConversationNode> Transactions { get; set; }
        public string IntroId { get; set; }
    }
}