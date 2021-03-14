import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { AnabranchMeta, Conversation, ConvoNode } from "@Palavyr-Types";
import { findIndex, sum } from "lodash";
import { _getNodeById, _getParentNode, _nodeListContainsNodeType, _splitAndRemoveEmptyNodeChildrenString, _splitNodeChildrenString } from "./_coreNodeUtils";

const Anabranch = "Anabranch".toUpperCase();

export const collectAnabranchMeta = (node: ConvoNode, nodeList: Conversation): AnabranchMeta => {
    let defaultResult = { isDecendentOfAnabranch: false, decendentLevelFromAnabranch: 0, nodeIdOfMostRecentAnabranch: "", isDirectChildOfAnabranch: false };

    if (!_nodeListContainsNodeType(nodeList, Anabranch)) {
        return defaultResult;
    }

    if (node.nodeType.toUpperCase() === Anabranch) {
        return defaultResult;
    }

    if (node.isRoot) {
        return defaultResult;
    }

    // defined
    let decendentLevelFromAnabranch = 0;
    let tempParentNode: ConvoNode | null = { ...node };
    let prevChildReference = { ...node };

    do {
        decendentLevelFromAnabranch++;
        prevChildReference = { ...tempParentNode! };
        tempParentNode = _getParentNode(prevChildReference, nodeList);
        if (tempParentNode === null) throw new Error("Orphan node detected");

        if (tempParentNode.nodeType.toUpperCase() === Anabranch) {
            return { isDecendentOfAnabranch: true, decendentLevelFromAnabranch, nodeIdOfMostRecentAnabranch: tempParentNode.nodeId, isDirectChildOfAnabranch: decendentLevelFromAnabranch == 1 };
        } else if (tempParentNode.isRoot) {
            return defaultResult;
        } else if (decendentLevelFromAnabranch > 200) {
            throw new Error("The tree is too deep (or the recursion algo is broken... either way, this cannot be allowed.");
        }
    } while (true);
};

const isChildOfSomeNode = (potentialParentId: string, currentNodeId: string, nodeList: Conversation) => {
    const currentNode = _getNodeById(currentNodeId, nodeList);

    if (currentNode.isRoot) {
        return false;
    }

    let tempParentNode: ConvoNode | null = { ...currentNode };
    let prevChildReference = { ...currentNode };
    let count = 0;
    do {
        count++;
        prevChildReference = { ...tempParentNode! };
        tempParentNode = _getParentNode(prevChildReference, nodeList);
        if (tempParentNode === null) throw new Error("Orphan node detected");

        if (tempParentNode.nodeId === potentialParentId) {
            return true;
        } else if (tempParentNode.isRoot) {
            return false;
        } else if (count > 200) {
            throw new Error("The tree is too deep (or the recursion algo is broken... either way, this cannot be allowed.");
        }
    } while (true);
};

export const allOtherSplitMergeTypesAreResolved = (node: ConvoNode, nodeList: Conversation) => {};

export const AllNonTerminalLeavesReferenceThisNode = (node: ConvoNode, nodeList: Conversation) => {};

export const otherNodeAlreadySetAsAnabranchMerge = (nodeId: string, nodeList: Conversation) => {
    const rootNode = _getNodeById(nodeId, nodeList);
    const childrenIds = _splitAndRemoveEmptyNodeChildrenString(rootNode.nodeChildrenString);
    let anabranchMergeIsSet = false;
    for (let index = 0; index < childrenIds.length; index++) {
        const childId = childrenIds[index];
        const childNode = _getNodeById(childId, nodeList);
        if (childNode.isAnabranchMergePoint) {
            anabranchMergeIsSet = true;
        } else {
            anabranchMergeIsSet = otherNodeAlreadySetAsAnabranchMerge(childNode.nodeId, nodeList);
        }
    }
    return anabranchMergeIsSet;
};

const isNonTerminalLeafNode = (node: ConvoNode) => {
    if (node.isTerminalType) return false;
    if (node.isRoot) return false;
    if (node.isMultiOptionType) return false;
    if (node.isSplitMergeType) return false;
    if (_splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString).length > 0) return false;
    return true;
};

export const recursivelyReferenceCurrentNodeInNonTerminalLeafNodes = (currentNodeId: string, nodeList: Conversation, rootNodeId: string) => {
    const rootNode = _getNodeById(rootNodeId, nodeList);
    const rootNodeChildIDs = _splitAndRemoveEmptyNodeChildrenString(rootNode.nodeChildrenString);

    if (rootNodeId === currentNodeId) {
        return nodeList;
    } else if (rootNode.isMultiOptionType) {
        rootNodeChildIDs.forEach((childId: string) => {
            // if multioption type - skip
            nodeList = recursivelyReferenceCurrentNodeInNonTerminalLeafNodes(currentNodeId, nodeList, childId);
        });
    } else {
        const childIds = _splitNodeChildrenString(rootNode.nodeChildrenString);
        if (childIds.length !== 1) throw new Error("What other kind of node type is there?: " + rootNode.nodeChildrenString);

        const childNode = _getNodeById(childIds[0], nodeList);
        if (childNode.nodeId !== currentNodeId && childNode.nodeType === "") {
            rootNode.nodeChildrenString = currentNodeId;
            rootNode.shouldRenderChildren = false;
            nodeList = nodeList.filter((x: ConvoNode) => x.nodeId !== childIds[0])
        } else {
            nodeList = recursivelyReferenceCurrentNodeInNonTerminalLeafNodes(currentNodeId, nodeList, childIds[0]);
        }
    }
    return nodeList;
};


export const anyMultiChoiceTypesWithUnsetChildren = (nodeId: string, nodeList: Conversation, resultArray: boolean[] | null = null): boolean[] => {
    if (resultArray === null) {
        resultArray = [];
    }

    const node = _getNodeById(nodeId, nodeList);
    if (_splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString).length === 0) {
        resultArray.push(false);
    }
    if (node.isAnabranchMergePoint){
        resultArray.push(false)
    }
    else if (node.isMultiOptionType && !node.isAnabranchType) {
        const multiCheckResult = checkChildrenForUnsetChildren(node, nodeList);
        resultArray.push(multiCheckResult)

    } else {
        const childIds = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString);
        for (let childIdIndex = 0; childIdIndex < childIds.length; childIdIndex++) {
            const childId = childIds[childIdIndex];
            const results = anyMultiChoiceTypesWithUnsetChildren(childId, nodeList, resultArray);
            resultArray.push.apply(results);
        }
    }
    return resultArray;

}

const checkChildrenForUnsetChildren = (childNode: ConvoNode, nodeList: Conversation) => {
    let result = false;
    const granChildren = _splitAndRemoveEmptyNodeChildrenString(childNode.nodeChildrenString);
    if (childNode.isMultiOptionType) {
        granChildren.forEach((granChildId: string) => {
            const grandChildNode = _getNodeById(granChildId, nodeList);
            if (isNullOrUndefinedOrWhitespace(grandChildNode.nodeType)) {
                result = true;
            }
        });
    }
    return result;
};
