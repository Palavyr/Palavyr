using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Stores;

namespace Palavyr.API.Controllers.Conversation
{
    [Obsolete]
    public class GetIsSplitMergeTypeController : PalavyrBaseController
    {
        private readonly IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore;
        private readonly IPricingStrategyTypeLister pricingStrategyTypeLister;
        public const string Route = "configure-conversations/check-is-split-merge/{nodeType}";

        string GUIDPattern = @"[{(]?\b[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}\b[)}]?";

        public GetIsSplitMergeTypeController(
            IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore,
            ILogger<GetIsSplitMergeTypeController> logger,
            IPricingStrategyTypeLister pricingStrategyTypeLister
        )
        {
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.pricingStrategyTypeLister = pricingStrategyTypeLister;
        }

        [HttpGet(Route)]
        public async Task<bool> Get(string nodeType)
        {
            foreach (var defaultNodeType in DefaultNodeTypeOptions.DefaultNodeTypeOptionsList)
            {
                if (nodeType == defaultNodeType.Value)
                {
                    return defaultNodeType.IsSplitMergeType;
                }
            }

            // node is a dynamic table node type
            // Comes in as e.g. SelectOneFlat-234234-324-2342-324
            foreach (var dynamicTableType in pricingStrategyTypeLister.ListPricingStrategies())
            {
                if (nodeType.StartsWith(dynamicTableType.GetTableType()))
                {
                    var tableId = Regex.Match(nodeType, GUIDPattern, RegexOptions.IgnoreCase).Value;
                    var table = await dynamicTableMetaStore.Get(tableId, s => s.TableId);
                    if (table != null)
                    {
                        return false;
                    }
                }
            }

            throw new Exception("DefaultNodeType not found.");
        }
    }
}