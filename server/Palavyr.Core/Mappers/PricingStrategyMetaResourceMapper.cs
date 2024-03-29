﻿using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class PricingStrategyMetaResourceMapper : IMapToNew<PricingStrategyTableMeta, PricingStrategyTableMetaResource>
    {
        public async Task<PricingStrategyTableMetaResource> Map(PricingStrategyTableMeta @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new PricingStrategyTableMetaResource()
            {
                TableId = @from.TableId,
                TableType = @from.TableType,
                TableTag = @from.TableTag,
                IntentId = @from.IntentId,
                PrettyName = @from.PrettyName,
                UnitIdEnum = @from.UnitIdEnum
            };
        }
    }
}