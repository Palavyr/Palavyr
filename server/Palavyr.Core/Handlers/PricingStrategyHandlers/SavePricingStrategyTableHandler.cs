using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.PricingStrategyHandlers
{
    public class SavePricingStrategyTableHandler<T, TR, TCompiler>
        : IRequestHandler<SavePricingStrategyTableRequest<T, TR, TCompiler>, SavePricingStrategyTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, ITable, new()
        where TR : IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
    {
        private readonly IEntityStore<T> entityStore;
        private readonly IPricingStrategyTableCommandExecutor<T, TCompiler> executor;
        private readonly IMapToNew<T, TR> resourceMapper;
        private readonly IMapToNew<TR, T> newEntityMapper;
        private readonly IMapToPreExisting<TR, T> mapToExisting;

        public SavePricingStrategyTableHandler(
            IEntityStore<T> entityStore,
            IPricingStrategyTableCommandExecutor<T, TCompiler> executor,
            IMapToNew<T, TR> resourceMapper,
            IMapToNew<TR, T> newEntityMapper,
            IMapToPreExisting<TR, T> mapToExisting)
        {
            this.entityStore = entityStore;
            this.executor = executor;
            this.resourceMapper = resourceMapper;
            this.newEntityMapper = newEntityMapper;
            this.mapToExisting = mapToExisting;
        }

        public async Task<SavePricingStrategyTableResponse<TR>> Handle(SavePricingStrategyTableRequest<T, TR, TCompiler> request, CancellationToken cancellationToken)
        {
            var tableUpdate = await MapTable(request.PricingStrategyTableResource.TableData, cancellationToken);

            var rows = await executor.SaveTable(request.IntentId, request.TableId, request.PricingStrategyTableResource.TableTag, tableUpdate);

            var resource = await resourceMapper.MapMany(rows, cancellationToken);
            return new SavePricingStrategyTableResponse<TR>(resource);
        }

        private async Task<List<T>> MapTable(List<TR> tableRowResources, CancellationToken cancellationToken)
        {
            var update = new List<T>();
            foreach (var tableRowResource in tableRowResources)
            {
                if (tableRowResource.Id is null)
                {
                    var newEntry = await newEntityMapper.Map(tableRowResource, cancellationToken);
                    update.Add(newEntry);
                }
                else
                {
                    var entity = await entityStore.Get((int)tableRowResource.Id);
                    await mapToExisting.Map(tableRowResource, entity, cancellationToken);
                    update.Add(entity);
                }
            }

            return update;
        }
    }

    public class SavePricingStrategyTableRequest<T, TR, TCompiler> : IRequest<SavePricingStrategyTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, new()
        where TR : IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
    {
        public string IntentId { get; set; }
        public string TableId { get; set; }
        public PricingStrategyTable<TR> PricingStrategyTableResource { get; set; }
    }

    public class SavePricingStrategyTableResponse<TR> where TR : IPricingStrategyTableRowResource
    {
        public SavePricingStrategyTableResponse(IEnumerable<TR> resource) => Resource = resource;
        public IEnumerable<TR> Resource { get; set; }
    }
}