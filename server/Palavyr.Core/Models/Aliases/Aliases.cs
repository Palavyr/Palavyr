using System.Collections.Generic;
using PricingStrategyType = System.String;
using PricingStrategyNodeId = System.String;
using PricingStrategyResponse = System.String;

// DESIGN for the DTO from the widget.
//
// The node id refers to the node set in the conversationNOde table. When used to retrieve from the DB,
// this has a 'resolveOrder' property which is used to determine the order in which these responses
// should be used to resolve the final result.
//
// Typically each element of the outer list will have only one key-value pair (responses for a single pricing strategy table)
// The list may have multiple elements (if the user decides to include multiple pricing strategy tables)
//
// [
//     { // PricingStrategyResponses (list of pricing strategy response

//         "PricingStrategyType": [ // PricingStrategyResponse
//                      // -- PricingStrategyResponseParts
//             {[node.nodeId]: "Response Value"}, 1 -- PricingStrategyResponsePart
//             {[node.nodeId]: "Response Value"}, 2 -- PricingStrategyResponsePart
//             {[node.nodeId]: "Response Value"}  0 -- PricingStrategyResponsePart
//         ]
//     },
//     {
//         "SelectOneFlat-1231": [
//             {"SelectOneFlat-1231": "Ruby"}
//         ]
//     }
// ]

namespace Palavyr.Core.Models.Aliases
{
    public class PricingStrategyResponses : List<PricingStrategyResponse>
    {
    }

    // PricingStrategyType is the ConvoNode Col name (PricingStrategyType) and is like "SelectOneFlat-1231"
    public class PricingStrategyResponse : Dictionary<PricingStrategyType, PricingStrategyResponseParts>
    {
    }

    public class PricingStrategyResponseParts : List<PricingStrategyResponsePart>
    {
    }

    public class PricingStrategyResponsePart : Dictionary<PricingStrategyNodeId, string>
    {
    }
}