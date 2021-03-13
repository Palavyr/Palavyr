import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { ConvoNode, Conversation, MostRecentSplitMerge } from "@Palavyr-Types";
import { findIndex } from "lodash";
import { _createAndAddNewNodes, _getNodeById, _getParentNode, _removeNodeByID, _replaceNodeWithUpdatedNode, _splitAndRemoveEmptyNodeChildrenString, _splitNodeChildrenString } from "./_coreNodeUtils";

export const updateChildOfIsSplitMergeType = (node: ConvoNode, parentNode: ConvoNode, nodeList: Conversation, setNodes: (updatedNodeList: Conversation) => void) => {
    const primarySiblingId = getPrimarySiblingIdFromParent(parentNode);

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

export const getPrimarySiblingIdFromParent = (parentNode: ConvoNode) => {
    return parentNode.nodeChildrenString.split(",")[0];
};

export const getSiblingIndex = (parentNode: ConvoNode, node: ConvoNode) => {
    return findIndex(parentNode.nodeChildrenString.split(","), (id: string) => id === node.nodeId);
};

export const findMostRecentSplitMerge = (node: ConvoNode, nodeList: Conversation): MostRecentSplitMerge => {
    // returns either the most recent splitMerge type, or null
    const SplitMerge = "SplitMerge".toUpperCase();

    if (!nodeListContainsSplitmerge(nodeList)) {// early bail if no splitmerges
        return { isDecendentOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: []}
    }

    if (node.nodeType.toUpperCase() === SplitMerge) {
        return { isDecendentOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: []};
    }

    if (node.isRoot) {
        return { isDecendentOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: []};
    }

    let found = false;
    let parentNode: ConvoNode;
    let decendentLevelFromSplitMerge = 0;
    let tempParentNode: ConvoNode | null = { ...node };
    let prevChildReference = { ...node };
    let splitMergeRootSiblingIndex: number;
    let nodeIdOfMostRecentSplitMergePrimarySibling: string;
    let orderedChildren: Conversation;
    let result: MostRecentSplitMerge = { isDecendentOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: []};
    do {
        decendentLevelFromSplitMerge++;
        prevChildReference = { ...tempParentNode! };
        tempParentNode = _getParentNodeYOLO(prevChildReference!, nodeList);
        if (tempParentNode === null) throw new Error("Orphan node detected.");
        if (tempParentNode.nodeType.toUpperCase() === SplitMerge) {
            found = true;
            parentNode = tempParentNode;
            splitMergeRootSiblingIndex = getSiblingIndex(parentNode, prevChildReference);
            nodeIdOfMostRecentSplitMergePrimarySibling = getPrimarySiblingIdFromParent(parentNode);
            orderedChildren = getorderedChildrenFromParent(parentNode, nodeList);
            result = { isDecendentOfSplitMerge: true, decendentLevelFromSplitMerge, splitMergeRootSiblingIndex, nodeIdOfMostRecentSplitMergePrimarySibling, orderedChildren};
            break;
        } else if (tempParentNode.isRoot) {
            found = true;
        } else if (decendentLevelFromSplitMerge > 200) {
            throw new Error("The tree is too deep. This cannot be allowed.")
        }
    } while (!found);
    if (decendentLevelFromSplitMerge === 3){
        console.log("WOW");

    }
    return result;
};

export const nodeListContainsSplitmerge = (nodeList: Conversation) => {
    const nodeTypes = nodeList.map((node: ConvoNode) => node.nodeType.toUpperCase());
    return nodeTypes.includes("SplitMerge".toUpperCase());
}

export const getorderedChildrenFromParent = (parentNode: ConvoNode, nodeList: Conversation) => {

    const children = parentNode.nodeChildrenString.split(",");
    const orderedNodes: Conversation = [];
    children.forEach((c: string) => {
        let node = _getNodeById(c, nodeList);
        orderedNodes.push(node);
    })
    return orderedNodes;
}

export const _getParentNodeYOLO = (node: ConvoNode, nodeList: Conversation) => {
    // used when trying to find most recent splitmerge.
    if (node.isRoot) {
        return null;
    }
    const parent = nodeList.filter((n: ConvoNode) => {
        if (isNullOrUndefinedOrWhitespace(n.nodeChildrenString)) {
            return false;
        }
        let children = n.nodeChildrenString.split(",");
        return children.includes(node.nodeId);
    });

    return parent[0];
};

export const childHasAtLeastOneChild = (node: ConvoNode, nodeList: Conversation) => {
    if (_splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString).length === 0) {
        return false;
    }
    const childrenIds = _splitNodeChildrenString(node.nodeChildrenString);

    for (let i = 0; i < childrenIds.length; i++) {
        let childnode = _getNodeById(childrenIds[i], nodeList)
        if (_splitAndRemoveEmptyNodeChildrenString(childnode.nodeChildrenString).length > 0) return true;
    }
    return true;
}