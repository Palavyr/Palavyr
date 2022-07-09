using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.PricingStrategyHandlers
{
    public class DeletePricingStrategyTableHandler<T, TR, TCompiler>
        : IRequestHandler<DeletePricingStrategyTableRequest<T, TR, TCompiler>, DeletePricingStrategyTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, ITable, new()
        where TR : class, IPricingStrategyTableRowResource
        where TCompiler : class, IPricingStrategyTableCompiler
    {
        private readonly IPricingStrategyTableCommandExecutor<T, TR, TCompiler> executor;

        public DeletePricingStrategyTableHandler(IPricingStrategyTableCommandExecutor<T, TR, TCompiler> executor)
        {
            this.executor = executor;
        }

        public async Task<DeletePricingStrategyTableResponse<TR>> Handle(DeletePricingStrategyTableRequest<T, TR, TCompiler> notification, CancellationToken cancellationToken)
        {
            await executor.DeleteTable(notification.IntentId, notification.TableId);
            return new DeletePricingStrategyTableResponse<TR>(); // This returns nothing. It just facilitates the registration...
        }
    }

    public class DeletePricingStrategyTableRequest<T, TR, TCompiler> : IRequest<DeletePricingStrategyTableResponse<TR>>
        where TR : IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
    {
        public const string Route = "intent/{intentId}/table/{tableId}";

        public string IntentId { get; set; }
        public string TableId { get; set; }
    }

    public class DeletePricingStrategyTableResponse<TR>
        where TR : IPricingStrategyTableRowResource
    {
    }

    public class EmptyResponse : IPricingStrategyTableRowResource
    {
        public int? Id { get; set; }
        public string TableId { get; set; }
    }
}