import { Conversation, ConvoNode, NodeSetterWithHistory } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { _replaceNodeWithUpdatedNode } from "../../nodeUtils/_coreNodeUtils";

export const _handleUnsetCurrentNodeType = (node: ConvoNode, nodeList: Conversation, setNodes: NodeSetterWithHistory) => {
    let updatedNodeList = cloneDeep(nodeList);
    const newNode = cloneDeep(node);
    newNode.nodeType = "";
    newNode.nodeChildrenString = "";
    newNode.valueOptions = "";
    updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);
    setNodes(cloneDeep([...updatedNodeList]));
};
