﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.Core.Handlers
{
    public class PerformInternalCheckHandler : IRequestHandler<PerformInternalCheckRequest, PerformInternalCheckResponse>
    {
        private readonly IDynamicResponseComponentExtractor dynamicResponseComponentExtractor;

        public PerformInternalCheckHandler(
            IDynamicResponseComponentExtractor dynamicResponseComponentExtractor
        )
        {
            this.dynamicResponseComponentExtractor = dynamicResponseComponentExtractor;
        }

        public async Task<PerformInternalCheckResponse> Handle(PerformInternalCheckRequest request, CancellationToken cancellationToken)
        {
            var dynamicResponseComponents = dynamicResponseComponentExtractor
                .ExtractDynamicTableComponents(request.CurrentDynamicResponseState);
            var result = await dynamicResponseComponents.Compiler.PerformInternalCheck(
                request.Node,
                request.Response,
                dynamicResponseComponents
            );
            return new PerformInternalCheckResponse(result);
        }
    }

    public class PerformInternalCheckResponse
    {
        public PerformInternalCheckResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class PerformInternalCheckRequest : IRequest<PerformInternalCheckResponse>
    {
        public PerformInternalCheckRequest(ConversationNode node, string response, DynamicResponse currentDynamicResponseState)
        {
            Node = node;
            Response = response;
            CurrentDynamicResponseState = currentDynamicResponseState;
        }

        public ConversationNode Node { get; set; }
        public string Response { get; set; }
        public DynamicResponse CurrentDynamicResponseState { get; set; }
    }
}