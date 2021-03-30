using System.Collections.Generic;
using Palavyr.Domain.Configuration.Schemas.DynamicTables;

namespace Palavyr.Domain.Resources.Requests
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