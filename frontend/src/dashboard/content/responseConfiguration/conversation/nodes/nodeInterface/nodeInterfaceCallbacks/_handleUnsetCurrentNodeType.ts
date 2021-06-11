import { Conversation, ConvoNode, NodeSetterWithHistory } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { _removeNodeByID, _replaceNodeWithUpdatedNode, _splitAndRemoveEmptyNodeChildrenString } from "../../nodeUtils/_coreNodeUtils";

export const _handleUnsetCurrentNodeType = (node: ConvoNode, nodeList: Conversation, setNodes: NodeSetterWithHistory) => {
    let updatedNodeList = cloneDeep(nodeList);
    const newNode = cloneDeep(node);
    newNode.nodeType = "";

    _splitAndRemoveEmptyNodeChildrenString(newNode.nodeChildrenString).forEach((nodeId: string) => {
        updatedNodeList = _removeNodeByID(nodeId, updatedNodeList)
    })

    newNode.nodeChildrenString = "";
    newNode.valueOptions = "";
    updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);
    // setNodes(cloneDeep([...updatedNodeList]));
};
