import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { Conversation, ConvoNode, NodeOption, NodeTypeOptions } from "@Palavyr-Types";
import { findIndex, sum } from "lodash";
import { checkIfNodeIsBoundedByAnabranch } from "./AnabranchUtils";
import {
    _computeShouldRenderChildren,
    _createNewChildIDs,
    _getIdsToDeleteRecursively,
    _getLeftMostParentNode,
    _getNodeById,
    _removeNodeByID,
    _replaceNodeWithUpdatedNode,
    _resetOptionPaths,
    _splitAndRemoveEmptyNodeChildrenString,
    _splitNodeChildrenString,
    _truncateTheTreeAtSpecificNode,
} from "./_coreNodeUtils";

export const getRootNode = (nodeList: Conversation) => {
    return nodeList.filter((node) => node.isRoot === true)[0];
};

export const getNewNumChildren = (optionPaths: string[]) => {
    return optionPaths.filter((x) => x !== null && x !== "").length;
};

export const checkedNodeOptionList = (nodeOptionList: NodeTypeOptions, isDecendentOfSplitMerge: boolean, splitMergeRootSiblingIndex: number, isParentOfAnabranchMergePoint: boolean) => {
    // This next line is a defensive check
    //TODO: Perhaps these two if statements should be mutually exclusive.
    if (isDecendentOfSplitMerge && splitMergeRootSiblingIndex > 0 && isParentOfAnabranchMergePoint) throw new Error("MUtally Exclusive!");

    if (isDecendentOfSplitMerge && splitMergeRootSiblingIndex > 0) {
        const compatible = nodeOptionList.filter((option: NodeOption) => option.groupName === "Provide Info" || option.groupName === "Info Collection");
        return compatible;
    } else if (isParentOfAnabranchMergePoint) {
        const compatible = nodeOptionList.filter((option: NodeOption) => option.groupName === "Provide Info" || option.groupName === "Info Collection");
        return compatible;
    } else {
        return nodeOptionList;
    }
};

export const getChildNodesToRender = (node: ConvoNode, nodeList: Conversation) => {
    return node.isSplitMergeType || node.isAnabranchType ? getChildeNodesOrderedByChildString(node.nodeChildrenString, nodeList) : getChildNodesSortedByOptionPath(node.nodeChildrenString, nodeList);
};

export const getChildeNodesOrderedByChildString = (childrenIDs: string, nodeList: Conversation) => {
    const ids = childrenIDs.split(",");
    if (ids.length === 1 && ids[0] === "") return [];
    const unorderedNodes: Conversation = [];
    ids.forEach((id: string) => {
        let index = findIndex(nodeList, (n: ConvoNode) => n.nodeId === id);
        unorderedNodes.push(nodeList[index]);
    });
    return unorderedNodes;
};

export const getChildNodesSortedByOptionPath = (childrenIds: string, nodeList: Conversation) => {
    const unsortedChildNodes = getChildeNodesOrderedByChildString(childrenIds, nodeList);
    const getter = (x: ConvoNode) => x.optionPath.toUpperCase();
    return sortByPropertyAlphabetical(getter, unsortedChildNodes);
};

export const createAndReattachNewNodes = (currentNode: ConvoNode, nodeList: Conversation, newNumChildren: number) => {
    const currentChildNodeIds = currentNode.nodeChildrenString.split(",").filter((x: string) => x !== "" && x !== null && x !== undefined);

    let newNodeList = [...nodeList];
    let newChildNodeIds: string[];
    let childIdsToCreate: string[] = [];
    if (currentChildNodeIds.length < newNumChildren) {
        // if we are adding new nodes
        const diff = newNumChildren - currentChildNodeIds.length;
        const newChildIds = _createNewChildIDs(diff);
        childIdsToCreate = [...newChildIds];
        newChildNodeIds = [...currentChildNodeIds, ...newChildIds];
    } else if (currentChildNodeIds.length > newNumChildren) {
        newChildNodeIds = [...currentChildNodeIds.splice(0, newNumChildren)];

        const nodeIdsToTruncateFrom = [...currentChildNodeIds.splice(newNumChildren)];
        nodeIdsToTruncateFrom.forEach((nodeId: string) => {
            let nodeToTruncateFrom = _getNodeById(nodeId, newNodeList);
            newNodeList = _truncateTheTreeAtSpecificNode(nodeToTruncateFrom, newNodeList);
        });
    } else {
        newChildNodeIds = [...currentChildNodeIds];
    }

    return {
        newNodeList: newNodeList,
        newChildNodeIds: newChildNodeIds,
        childIdsToCreate: childIdsToCreate,
    };
};

export const updateSingleOptionType = (updatedNode: ConvoNode, nodeList: Conversation, setNodes: (nodeList: Conversation) => void) => {
    const updatedNodeList = _replaceNodeWithUpdatedNode(updatedNode, nodeList);
    setNodes(updatedNodeList);
};

export const nodeMergesToPrimarySibling = (node: ConvoNode, isDecendentOfSplitMerge: boolean, splitMergeRootSiblingIndex: number, nodeIdOfMostRecentSplitMergePrimarySibling: string | null) => {
    if (!isDecendentOfSplitMerge) return false;
    if (splitMergeRootSiblingIndex === 0) return false;

    const childNodeStrings = node.nodeChildrenString.split(",").filter((childId: string) => !isNullOrUndefinedOrWhitespace(childId));
    if (childNodeStrings.length !== 1) return false;

    return childNodeStrings[0] === nodeIdOfMostRecentSplitMergePrimarySibling;
};

export const determineIfCanUnsetNodeType = (node: ConvoNode, nodeList: Conversation, isDecendentOfAnabranch: boolean, anabranchId: string) => {
    // cannot  unset nodetype if: bounded AND is Terminal

    const nodeIsBoundedByAnabranch = checkIfNodeIsBoundedByAnabranch(node, nodeList, isDecendentOfAnabranch, anabranchId);
    if (nodeIsBoundedByAnabranch) {
        return false;
    }

    if (node.nodeChildrenString === "") {
        return true;
    }
    const childNodes = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString).map((childId: string) => _getNodeById(childId, nodeList));
    const result = sum(childNodes.map((childNode: ConvoNode) => childNode.nodeType !== "").map((x) => (x ? 1 : 0)));
    return result === 0;
};

export const determineIfIsOnLeftmostBranchGivenAnOriginNode = (targetNodeId: string, nodeList: Conversation, originNodeId: string) => {
    const rootNode = _getNodeById(originNodeId, nodeList);
    const childrenNodeIds = _splitAndRemoveEmptyNodeChildrenString(rootNode.nodeChildrenString);
    if (childrenNodeIds.length === 0) return false;
    else {
        if (childrenNodeIds[0] === targetNodeId) { // FOUND
            return true;
        } else {
            return determineIfIsOnLeftmostBranchGivenAnOriginNode(targetNodeId, nodeList, childrenNodeIds[0])
        }
    }
};
