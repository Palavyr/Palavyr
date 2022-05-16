using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.Core.Handlers.PricingStrategyHandlers
{
    public class DeletePricingStrategyTableHandler<T, TR>
        : IRequestHandler<DeletePricingStrategyTableRequest<T, TR>, DeletePricingStrategyTableResponse<TR>>
        where T : class, IDynamicTable<T>, IRequest<TR>, new()
        where TR : IPricingStrategyTableRowResource
    {
        private readonly IDynamicTableCommandExecutor<T> executor;

        public DeletePricingStrategyTableHandler(
            IDynamicTableCommandExecutor<T> executor
        )
        {
            this.executor = executor;
        }

        public async Task<DeletePricingStrategyTableResponse<TR>> Handle(DeletePricingStrategyTableRequest<T, TR> notification, CancellationToken cancellationToken)
        {
            await executor.DeleteDynamicTable(notification.IntentId, notification.TableId);
            return new DeletePricingStrategyTableResponse<TR>(); // This returns nothing. It just facilitates the registration...
        }
    }

    public class DeletePricingStrategyTableRequest<T, TR> : IRequest<DeletePricingStrategyTableResponse<TR>> where T : class
    {
        public string IntentId { get; set; }
        public string TableId { get; set; }
    }

    public class DeletePricingStrategyTableResponse<TR>
    {
    }

    public class EmptyResponse : IPricingStrategyTableRowResource
    {
        
    }
}