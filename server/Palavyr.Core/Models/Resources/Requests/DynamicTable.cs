﻿using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.Core.Models.Resources.Requests
{
    public class DynamicTable
    {
        public List<SelectOneFlat>? SelectOneFlat { get; set; } = new List<SelectOneFlat>();
        public List<PercentOfThreshold>? PercentOfThreshold { get; set; } = new List<PercentOfThreshold>();
        public List<BasicThreshold>? BasicThreshold { get; set; } = new List<BasicThreshold>();
        public List<TwoNestedCategory>? TwoNestedCategory { get; set; } = new List<TwoNestedCategory>();

        // add new Dynamic Table type rows properties
        public string? TableTag { get; set; }
    }
}