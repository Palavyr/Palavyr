﻿using System.Collections.Generic;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.RequestTypes
{
    public class DynamicTable
    {
        public List<SelectOneFlat> SelectOneFlat { get; set; }
        public List<PercentOfThreshold> PercentOfThreshold { get; set; }
 
        // add new Dynamic Table type rows properties
        public string TableTag { get; set; }
    }
}