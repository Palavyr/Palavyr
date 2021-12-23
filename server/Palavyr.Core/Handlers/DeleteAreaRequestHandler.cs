using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.Deletion;

namespace Palavyr.Core.Handlers
{
    public class DeleteAreaRequestHandler : IRequestHandler<DeleteAreaRequest>
    {
        private readonly IAreaDeleter areaDeleter;

        public DeleteAreaRequestHandler(IAreaDeleter areaDeleter)
        {
            this.areaDeleter = areaDeleter;
        }

        public async Task<Unit> Handle(DeleteAreaRequest request, CancellationToken cancellationToken)
        {
            await areaDeleter.DeleteArea(request.AreaId, cancellationToken);
            return default;
        }
    }
    
    public class DeleteAreaRequest : IRequest
    {
        public string AreaId { get; set; }
    }
}