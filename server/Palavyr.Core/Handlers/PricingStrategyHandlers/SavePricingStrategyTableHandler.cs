using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.PricingStrategyHandlers
{
    public class SavePricingStrategyTableHandler<T, TR, TCompiler>
        : IRequestHandler<SavePricingStrategyTableRequest<T, TR, TCompiler>, SavePricingStrategyTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, ITable, new()
        where TR : class, IPricingStrategyTableRowResource
        where TCompiler : class, IPricingStrategyTableCompiler
    {
        private readonly IEntityStore<T> entityStore;
        private readonly IPricingStrategyTableCommandExecutor<T, TR, TCompiler> executor;
        private readonly IMapToNew<T, TR> resourceMapper;
        private readonly IMapToNew<TR, T> newEntityMapper;
        private readonly IValidator<PricingStrategyTableDataResource<TR>> pricingStrategyValidator;
        private readonly IMapToPreExisting<TR, T> mapToExisting;

        public SavePricingStrategyTableHandler(
            IEntityStore<T> entityStore,
            IPricingStrategyTableCommandExecutor<T, TR, TCompiler> executor,
            IMapToNew<T, TR> resourceMapper,
            IMapToNew<TR, T> newEntityMapper,
            IValidator<PricingStrategyTableDataResource<TR>> pricingStrategyValidator,
            IMapToPreExisting<TR, T> mapToExisting)
        {
            this.entityStore = entityStore;
            this.executor = executor;
            this.resourceMapper = resourceMapper;
            this.newEntityMapper = newEntityMapper;
            this.pricingStrategyValidator = pricingStrategyValidator;
            this.mapToExisting = mapToExisting;
        }

        public async Task<SavePricingStrategyTableResponse<TR>> Handle(SavePricingStrategyTableRequest<T, TR, TCompiler> request, CancellationToken cancellationToken)
        {
            var validationResult = await pricingStrategyValidator.ValidateAsync(request.PricingStrategyTableResource, cancellationToken);
            if (!validationResult.IsValid) throw new MultiMessageDomainException("Bad Request: ", validationResult.Errors.Select(x => x.ErrorMessage).Distinct().ToArray());

            var tableUpdate = await MapTable(request.PricingStrategyTableResource, cancellationToken);

            var rows = await executor.SaveTable(request.IntentId, request.TableId, request.PricingStrategyTableResource.TableTag, tableUpdate);

            var resource = await resourceMapper.MapMany(rows, cancellationToken); // TODO - this should give me a PricingStrategyTableResource<TR>
            return new SavePricingStrategyTableResponse<TR>(new PricingStrategyTableDataResource<TR>(resource.ToList(), request.PricingStrategyTableResource.TableTag, request.PricingStrategyTableResource.IsInUse));
        }

        private async Task<List<T>> MapTable(PricingStrategyTableDataResource<TR> tableRowResources, CancellationToken cancellationToken)
        {
            var update = new List<T>();
            foreach (var tableRowResource in tableRowResources.TableRows)
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
        where TR : class, IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
    {
        public const string Route = "intent/{intentId}/table/{tableId}";

        public static string FormatRoute(string intentId, string tableId)
        {
            return Route.Replace("{intentId}", intentId).Replace("{tableId}", tableId);
        }

        public string IntentId { get; set; }
        public string TableId { get; set; }
        public PricingStrategyTableDataResource<TR> PricingStrategyTableResource { get; set; }

        public SavePricingStrategyTableRequest()
        {
        }
    }

    public class SavePricingStrategyTableResponse<TR> where TR : IPricingStrategyTableRowResource
    {
        public SavePricingStrategyTableResponse(PricingStrategyTableDataResource<TR> resource) => Resource = resource;
        public PricingStrategyTableDataResource<TR> Resource { get; set; }
    }
}