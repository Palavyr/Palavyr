using System.Collections.Generic;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public class PricingStrategyTableDataResource<TResource> where TResource : IPricingStrategyTableRowResource
    {
        public PricingStrategyTableDataResource()
        {
        }

        public PricingStrategyTableDataResource(List<TResource> tableRows, string tableTag, bool isInUse) : this()
        {
            TableRows = tableRows;
            TableTag = tableTag;
            IsInUse = isInUse;
        }

        public List<TResource> TableRows { get; set; }
        public bool IsInUse { get; set; }
        public string TableTag { get; set; }

        public void AddRows(List<TResource> rows)
        {
            TableRows.AddRange(rows);
        }

        public void AddRow(TResource row)
        {
            TableRows.Add(row);
        }
    }
}