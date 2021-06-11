import { ConvoNode, Conversation, NodeSetterWithHistory } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Dispatch, SetStateAction } from "react";
import { recursivelyReferenceCurrentNodeInNonTerminalLeafNodes } from "../../nodeUtils/AnabranchUtils";
import { recursivelyDereferenceNodeIdFromChildrenExceptWhen } from "../../nodeUtils/dereferenceUtils";
import { _replaceNodeWithUpdatedNode, _getNodeById, _getLeftMostParentNode } from "../../nodeUtils/_coreNodeUtils";

export const setNodeAsAnabranchMergePoint = (node: ConvoNode, nodeList: Conversation, nodeIdOfMostRecentAnabranch: string, setAnabranchMergeChecked: Dispatch<SetStateAction<boolean>>) => {
    const newNode = cloneDeep(node);
    let updatedNodeList = cloneDeep(nodeList);

    newNode.isAnabranchMergePoint = true;
    updatedNodeList = recursivelyReferenceCurrentNodeInNonTerminalLeafNodes(newNode.nodeId, updatedNodeList, nodeIdOfMostRecentAnabranch);
    updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);
    setAnabranchMergeChecked(true);
    return updatedNodeList;
};

export const unsetNodeAsAnabranchMergePoint = (node: ConvoNode, nodeList: Conversation, nodeIdOfMostRecentAnabranch: string, setAnabranchMergeChecked: Dispatch<SetStateAction<boolean>>) => {
    let updatedNodeList = cloneDeep(nodeList);
    const newNode = cloneDeep(node);
    newNode.isAnabranchMergePoint = false;
    updatedNodeList = _replaceNodeWithUpdatedNode(newNode, updatedNodeList);

    const anabranchRootNode = _getNodeById(nodeIdOfMostRecentAnabranch, updatedNodeList);
    const leftmostParentNode = _getLeftMostParentNode(node, nodeList, (node: ConvoNode) => node.isAnabranchType);
    if (leftmostParentNode) {
        recursivelyDereferenceNodeIdFromChildrenExceptWhen(leftmostParentNode.nodeId, anabranchRootNode, updatedNodeList, newNode.nodeId);
        setAnabranchMergeChecked(false);
        return updatedNodeList;
    }
    return null;
};

export const _handleSetAsAnabranchMergePointClick = (checked: boolean, node: ConvoNode, nodeList: Conversation, nodeIdOfMostRecentAnabranch: string, setAnabranchMergeChecked: Dispatch<SetStateAction<boolean>>, setNodes: NodeSetterWithHistory) => {
    let updatedNodeList: Conversation | null;
    if (checked) {
        updatedNodeList = setNodeAsAnabranchMergePoint(node, nodeList, nodeIdOfMostRecentAnabranch, setAnabranchMergeChecked);
    } else {
        updatedNodeList = unsetNodeAsAnabranchMergePoint(node, nodeList, nodeIdOfMostRecentAnabranch, setAnabranchMergeChecked);
    }
    if (updatedNodeList) {
        // setNodes(cloneDeep(updatedNodeList));
    }
};
