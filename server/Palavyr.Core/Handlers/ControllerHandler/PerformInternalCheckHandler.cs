using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class PerformInternalCheckHandler : IRequestHandler<PerformInternalCheckRequest, PerformInternalCheckResponse>
    {
        private readonly IPricingStrategyResponseComponentExtractor pricingStrategyResponseComponentExtractor;

        public PerformInternalCheckHandler(
            IPricingStrategyResponseComponentExtractor pricingStrategyResponseComponentExtractor
        )
        {
            this.pricingStrategyResponseComponentExtractor = pricingStrategyResponseComponentExtractor;
        }

        public async Task<PerformInternalCheckResponse> Handle(PerformInternalCheckRequest request, CancellationToken cancellationToken)
        {
            var dynamicResponseComponents = pricingStrategyResponseComponentExtractor
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
        public ConversationNode Node { get; set; }
        public string Response { get; set; }
        public DynamicResponse CurrentDynamicResponseState { get; set; }
    }
}