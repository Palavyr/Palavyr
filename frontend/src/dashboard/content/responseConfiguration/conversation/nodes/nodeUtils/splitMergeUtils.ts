import { ConvoNode, Conversation, SplitMergeMeta } from "@Palavyr-Types";
import { findIndex } from "lodash";
import { _getNodeById, _getParentNode, _joinNodeChildrenStringArray, _nodeListContainsNodeType, _removeNodeByID, _replaceNodeWithUpdatedNode, _splitAndRemoveEmptyNodeChildrenString, _splitNodeChildrenString } from "./_coreNodeUtils";

const SplitMerge = "SplitMerge".toUpperCase();

export const updateChildOfIsSplitMergeType = (node: ConvoNode, parentNode: ConvoNode, nodeList: Conversation, setNodes: (updatedNodeList: Conversation) => void) => {
    const primarySiblingId = getPrimarySiblingIdFromChildNodeChildrenString(parentNode);

    node.nodeChildrenString = primarySiblingId;
    node.isMultiOptionType = false;
    node.isTerminalType = false;
    node.isSplitMergeType = false;

    node.valueOptions = "";

    // remove old parent node from nodelist
    nodeList = _removeNodeByID(node.nodeId, nodeList);

    // add updated node to nodelist
    nodeList.push(node);

    delete node.id;

    setNodes(nodeList);
};

export const getPrimarySiblingIdFromChildNodeChildrenString = (parentNode: ConvoNode) => {
    return parentNode.nodeChildrenString.split(",")[0];
};

export const getSiblingIndex = (parentNode: ConvoNode, node: ConvoNode) => {
    return findIndex(parentNode.nodeChildrenString.split(","), (id: string) => id === node.nodeId);
};

export const collectSplitMergeMeta = (node: ConvoNode, nodeList: Conversation): SplitMergeMeta => {
    let defaultResult = { isDecendentOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: ""};

    if (!_nodeListContainsNodeType(nodeList, SplitMerge)) {
        // early bail if no splitmerges
        return defaultResult;
    }

    if (node.nodeType.toUpperCase() === SplitMerge) {
        return defaultResult;
    }

    if (node.isRoot) {
        return defaultResult;
    }

    let found = false;
    let parentNode: ConvoNode;
    let decendentLevelFromSplitMerge = 0;
    let tempParentNode: ConvoNode | null = { ...node };
    let prevChildReference = { ...node };
    let splitMergeRootSiblingIndex: number;
    let nodeIdOfMostRecentSplitMergePrimarySibling: string;
    let result: SplitMergeMeta = defaultResult;
    do {
        decendentLevelFromSplitMerge++;
        prevChildReference = { ...tempParentNode };
        tempParentNode = _getParentNode(prevChildReference, nodeList);
        if (tempParentNode === null) throw new Error("Orphan node detected.");
        if (tempParentNode.nodeType.toUpperCase() === SplitMerge) {
            found = true;
            parentNode = tempParentNode;
            splitMergeRootSiblingIndex = getSiblingIndex(parentNode, prevChildReference);
            nodeIdOfMostRecentSplitMergePrimarySibling = getPrimarySiblingIdFromChildNodeChildrenString(parentNode);
            result = { isDecendentOfSplitMerge: true, decendentLevelFromSplitMerge, splitMergeRootSiblingIndex, nodeIdOfMostRecentSplitMergePrimarySibling };
            break;
        } else if (tempParentNode.isRoot) {
            found = true;
        } else if (decendentLevelFromSplitMerge > 200) {
            throw new Error("The tree is too deep. This cannot be allowed.");
        }
    } while (!found);
    return result;
};

export const nodeListContainsSplitmerge = (nodeList: Conversation) => {
    const nodeTypes = nodeList.map((node: ConvoNode) => node.nodeType.toUpperCase());
    return nodeTypes.includes("SplitMerge".toUpperCase());
};

export const childHasAtLeastOneChild = (node: ConvoNode, nodeList: Conversation) => {
    if (_splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString).length === 0) {
        return false;
    }
    const childrenIds = _splitNodeChildrenString(node.nodeChildrenString);

    for (let i = 0; i < childrenIds.length; i++) {
        let childnode = _getNodeById(childrenIds[i], nodeList);
        if (_splitAndRemoveEmptyNodeChildrenString(childnode.nodeChildrenString).length > 0) return true;
    }
    return true;
};
