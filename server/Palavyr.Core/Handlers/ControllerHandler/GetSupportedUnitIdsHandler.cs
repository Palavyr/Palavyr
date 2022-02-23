using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.Units;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetSupportedUnitIdsHandler : IRequestHandler<GetSupportedUnitIdsRequest, GetSupportedUnitIdsResponse>
    {
        private readonly IUnitRetriever unitRetriever;
        private readonly Units units;

        public GetSupportedUnitIdsHandler(IUnitRetriever unitRetriever)
        {
            this.unitRetriever = unitRetriever;
        }

        public async Task<GetSupportedUnitIdsResponse> Handle(GetSupportedUnitIdsRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var supportedUnitIds = unitRetriever.GetUnitDefinitions();
            return new GetSupportedUnitIdsResponse(supportedUnitIds);
        }
    }

    public class GetSupportedUnitIdsRequest : IRequest<GetSupportedUnitIdsResponse>
    {
    }

    public class GetSupportedUnitIdsResponse
    {
        public List<QuantUnit> Response { get; set; }
        public GetSupportedUnitIdsResponse(List<QuantUnit> response) => Response = response;
    }
}