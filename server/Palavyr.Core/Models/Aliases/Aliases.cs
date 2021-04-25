using System.Collections.Generic;
using DynamicType = System.String;
using DynamicNodeId = System.String;
using DynamicResponse = System.String;

// DESIGN for the DTO from the widget.
//
// The node id refers to the node set in the conversationNOde table. When used to retrieve from the DB,
// this has a 'resolveOrder' property which is used to determine the order in which these responses
// should be used to resolve the final result.
//
// Typically each element of the outer list will have only one key-value pair (responses for a single dynamic table)
// The list may have multiple elements (if the user decides to include multiple dynamic tables)
//
// [
//     { // DynamicResponses (list of dynamic response

//         "DynamicType": [ // DynamicResponse
//                      // -- DynamicResponseParts
//             {[node.nodeId]: "Response Value"}, 1 -- DynamicResponsePart
//             {[node.nodeId]: "Response Value"}, 2 -- DynamicResponsePart
//             {[node.nodeId]: "Response Value"}  0 -- DynamicResponsePart
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
    public class DynamicResponses : List<DynamicResponse>
    {
    }

    // DynamicType is the ConvoNode Col name (DynamicType) and is like "SelectOneFlat-1231"
    public class DynamicResponse : Dictionary<DynamicType, DynamicResponseParts>
    {
    }

    public class DynamicResponseParts : List<DynamicResponsePart>
    {
    }

    public class DynamicResponsePart : Dictionary<DynamicNodeId, string>
    {
    }
}