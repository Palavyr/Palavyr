using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.Core.Models.Resources.Requests
{
    public class DynamicTable
    {
        public List<SelectOneFlat> SelectOneFlat { get; set; }
        public List<PercentOfThreshold> PercentOfThreshold { get; set; }
        public List<BasicThreshold> BasicThreshold {get; set; }
        public List<TwoNestedCategory> TwoNestedCategory { get; set; }
        
        // add new Dynamic Table type rows properties
        public string TableTag { get; set; }
    }
}