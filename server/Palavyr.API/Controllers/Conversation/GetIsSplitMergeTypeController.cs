﻿using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.API.Controllers.Conversation
{
    [Obsolete]
    public class GetIsSplitMergeTypeController : PalavyrBaseController
    {
        public const string Route = "configure-conversations/check-is-split-merge/{nodeType}";

        string GUIDPattern = @"[{(]?\b[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}\b[)}]?";
        private DashContext dashContext;

        public GetIsSplitMergeTypeController(
            ILogger<GetIsSplitMergeTypeController> logger,
            DashContext dashContext
        )
        {
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
            foreach (var dynamicTableType in DynamicTableTypes.GetDynamicTableTypes())
            {
                if (nodeType.StartsWith(dynamicTableType.TableType))
                {
                    var tableId = Regex.Match(nodeType, GUIDPattern, RegexOptions.IgnoreCase).Value;
                    var table = await dashContext
                        .DynamicTableMetas
                        .SingleOrDefaultAsync(row => row.TableId == tableId);
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