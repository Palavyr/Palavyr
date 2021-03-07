import { Conversation, ConvoNode, NodeOption, Responses } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { ApiClient } from "@api-client/Client";
import { createAndReattachNewNodes, getNewNumChildren } from "./nodeUtils/commonNodeUtils";
import { _removeNodeByID } from "./nodeUtils/_coreNodeUtils";

// export const removeNodes = (nodeList: Conversation, nodeId: string, setNodes: (nodeList: Conversation) => void) => {
//     // removes the node by nodeId
//     let currentMeta = nodeList.filter((node: ConvoNode) => node.nodeId === nodeId).pop() as ConvoNode;
//     if (currentMeta.isRoot) {
//         return false;
//     } else {
//         nodeList = nodeList.filter((node) => node.nodeId !== nodeId);
//         setNodes(cloneDeep(nodeList));
//     }
// };

// export const changeNodeType = async (node: ConvoNode, nodeList: Conversation, optionPaths: Responses, valueOptions: Array<string>, setNodes: (nodeList: Conversation) => void, nodeOption: NodeOption) => {
//     // var client = new ApiClient();

//     const newNumChildren = getNewNumChildren(optionPaths);
//     const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(node, nodeList, newNumChildren);

//     // // reset the parentNode's children
//     // if (parentNode?.isSplitMergeType && siblingIndex > 0) {
//     //     const primarySibling = getPrimarySibling(parentNode, node);
//     //     node.nodeChildrenString = primarySibling;
//     // } else {
//     //     node.nodeChildrenString = newChildNodeIds.join(",");
//     // }

//     // TODO: delete this after substantial testing. I think this is okay to go since
//     // the parent node is the same node that is being passed in, and should have the updated text.
//     // map old node text to new node
//     // var n = nodeList.filter((x) => x.nodeId === parentNode.nodeId)[0];
//     // parentNode.text = n.text;

//     // is the new parent a multiOptionNodeType
//     // const { data: isMultiOptionType } = await client.Conversations.CheckIfIsMultiOptionType(node.nodeType);
//     node.isMultiOptionType = nodeOption.isMultiOptionType;

//     // const { data: isTerminalType } = await client.Conversations.CheckIfIsTerminalType(node.nodeType);
//     node.isTerminalType = nodeOption.isTerminalType;

//     // const { data: isSplitMergetype } = await client.Conversations.CheckIfIsSplitMergeType(node.nodeType);
//     node.isSplitMergeType = nodeOption.isSplitMergeType;

//     // remove old parent node from nodelist
//     const updatedNodeList = _removeNodeByID(node.nodeId, newNodeList);

//     // add updated node to nodelist
//     nodeList.push(node);

//     delete node.id;

//     // set any value options
//     node.valueOptions = valueOptions.join("|peg|"); // TODO: get this seperator from server

//     childIdsToCreate.forEach((id: string, index: number) => {
//         let newNode: ConvoNode = {
//             nodeId: id, // replace with uuid
//             nodeType: "", // default
//             text: "Ask your question!",
//             nodeChildrenString: "",
//             isRoot: false,
//             fallback: false,
//             isCritical: false,
//             areaIdentifier: node.areaIdentifier,
//             optionPath: optionPaths[index],
//             valueOptions: "",
//             isMultiOptionType: false,
//             isTerminalType: false,
//             isSplitMergeType: false,
//             shouldRenderChildren: computeShouldRenderChildren(node, index),
//         };

//         updatedNodeList.push(newNode);
//     });

//     setNodes(updatedNodeList);
// };

// const computeShouldRenderChildren = (parentNode: ConvoNode, index: number) => {
//     if (parentNode.isSplitMergeType) {
//         if (index === 0) {
//             return true;
//         } else {
//             return false;
//         }
//     } else {
//         return true;
//     }
// };
