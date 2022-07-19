﻿using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class PercentOfThresholdMapper : IMapToNew<PercentOfThresholdResource, PercentOfThresholdTableRow>
    {
        public async Task<PercentOfThresholdTableRow> Map(PercentOfThresholdResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new PercentOfThresholdTableRow
            {
                Id = from.Id,
                Modifier = from.Modifier,
                Range = from.Range,
                Threshold = from.Threshold,
                AccountId = from.AccountId,
                IntentId = from.IntentId,
                ItemId = from.ItemId,
                Category = from.ItemName,
                ItemOrder = from.ItemOrder,
                PosNeg = from.PosNeg,
                RowId = from.RowId,
                RowOrder = from.RowOrder,
                TableId = from.TableId,
                TriggerFallback = from.TriggerFallback,
                ValueMax = from.ValueMax,
                ValueMin = from.ValueMin
            };
        }
    }
}