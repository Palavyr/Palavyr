﻿using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Resources.PricingStrategyResources
{
    public interface IPricingStrategyTableRowResource : IId
    {
        public string TableId { get; set; }
    }
}