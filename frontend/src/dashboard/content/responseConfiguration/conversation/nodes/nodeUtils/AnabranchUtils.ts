import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { AnabranchMeta, Conversation, ConvoNode } from "@Palavyr-Types";
import { sum, uniqBy } from "lodash";
import { _getNodeById, _getParentNode, _nodeListContainsNodeType, _splitAndRemoveEmptyNodeChildrenString, _splitNodeChildrenString } from "./_coreNodeUtils";

const Anabranch = "Anabranch".toUpperCase();

export const checkIfSitsWithinOpenAnabranch = (nodeList: Conversation, isDecendantOfAnabranch: boolean, nodeIdOfMostRecentAnabranch: string) => {
    if (!isDecendantOfAnabranch) return false;
    // if is decendant of anabranch, check if there is a mergepoint down the chain
    const mostRecentAnabranchNode = _getNodeById(nodeIdOfMostRecentAnabranch, nodeList);
    // if we are not ancestor of anabranch  merge point, then its an open anabranch.
    return !checkIfIsAncestorOfAnabranchMergePoint(mostRecentAnabranchNode, nodeList);
};

export const checkIfNodeIsBoundedByAnabranch = (nodeList: Conversation, isDecendentOfAnabranch: boolean, anabranchId: string) => {
    if (!isDecendentOfAnabranch) {
        return false;
    }
    const anabranchNode = _getNodeById(anabranchId, nodeList);
    const anabranchIsRemerged = checkIfIsAncestorOfAnabranchMergePoint(anabranchNode, nodeList);

    // if we have a terminal type that is a decendent, and the anabranch is closed, then this won't return true from that (when it should)

    return isDecendentOfAnabranch && anabranchIsRemerged;
};

export const checkIfDecendentOfAnabranch = (node: ConvoNode, nodeList: Conversation) => {
    let tempParentNode: ConvoNode | null = { ...node };
    let prevChildReference = { ...node };
    let isDecendent: boolean | undefined;
    let count = 0;

    if (node.isRoot) {
        return false;
    }

    do {
        count++;
        prevChildReference = { ...tempParentNode! };
        tempParentNode = _getParentNode(prevChildReference, nodeList);
        if (tempParentNode === null) throw new Error("Orphan node detected");
        if (tempParentNode.isRoot) {
            isDecendent = false;
        } else if (tempParentNode.isAnabranchType) {
            isDecendent = true;
        } else if (count > 200) {
            throw new Error("The tree is too deep (or the recursion algo is broken... either way, this cannot be allowed.");
        }
    } while (isDecendent === undefined);

    return isDecendent;
};

export const checkIfIsAncestorOfAnabranchMergePoint = (node: ConvoNode, nodeList: Conversation, isAncestor: boolean | null = null): boolean => {
    if (isAncestor === null) {
        isAncestor = false;
    }

    const childrenIds = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString);
    for (let index = 0; index < childrenIds.length; index++) {
        const childId = childrenIds[index];
        const childNode = _getNodeById(childId, nodeList);
        if (childNode.isAnabranchMergePoint) {
            isAncestor = true;
        } else {
            isAncestor = checkIfIsAncestorOfAnabranchMergePoint(childNode, nodeList, isAncestor);
        }
    }
    return isAncestor;
};

export const collectAnabranchMeta = (node: ConvoNode, nodeList: Conversation): AnabranchMeta => {
    const isParentOfAnabranchMergePoint = checkIfIsParentOfAnabranchMergePoint(node, nodeList);
    const isAncestorOfAnabranchMergePoint = checkIfIsAncestorOfAnabranchMergePoint(node, nodeList);
    let defaultResult = { isDecendentOfAnabranch: false, decendentLevelFromAnabranch: 0, nodeIdOfMostRecentAnabranch: "", isDirectChildOfAnabranch: false, isParentOfAnabranchMergePoint, isAncestorOfAnabranchMergePoint };

    if (!_nodeListContainsNodeType(nodeList, Anabranch)) {
        return defaultResult;
    }

    if (node.nodeType.toUpperCase() === Anabranch && !node.isAnabranchMergePoint) {
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
            return {
                isDecendentOfAnabranch: true,
                decendentLevelFromAnabranch,
                nodeIdOfMostRecentAnabranch: tempParentNode.nodeId,
                isDirectChildOfAnabranch: decendentLevelFromAnabranch == 1,
                isParentOfAnabranchMergePoint,
                isAncestorOfAnabranchMergePoint,
            };
        } else if (tempParentNode.isAnabranchMergePoint) {
            return defaultResult;
        } else if (tempParentNode.isRoot) {
            return defaultResult;
        } else if (decendentLevelFromAnabranch > 200) {
            throw new Error("The tree is too deep (or the recursion algo is broken... either way, this cannot be allowed.");
        }
    } while (true);
};

const checkIfIsParentOfAnabranchMergePoint = (node: ConvoNode, nodeList: Conversation) => {
    const childrenIds = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString);
    if (childrenIds.length === 0) return false;
    let isParentOfAnabranchMergePoint = false;
    for (let index = 0; index < childrenIds.length; index++) {
        const childId = childrenIds[index];
        const node = _getNodeById(childId, nodeList);
        if (node.isAnabranchMergePoint) {
            isParentOfAnabranchMergePoint = true;
        }
    }
    return isParentOfAnabranchMergePoint;
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

export const otherNodeAlreadySetAsAnabranchMerge = (nodeId: string, nodeList: Conversation, currentNodeId: string) => {
    const rootNode = _getNodeById(nodeId, nodeList);
    console.log(rootNode);
    const childrenNodes: Conversation = [];
    const result: Conversation = uniqBy(gatherChildren(rootNode, nodeList, childrenNodes, currentNodeId), (x: ConvoNode) => x.nodeId);
    const anabranchMerges = result.map((node: ConvoNode) => (node.isAnabranchMergePoint ? 1 : 0));
    return sum(anabranchMerges) > 0;
};

const gatherChildren = (node: ConvoNode, nodeList: Conversation, children: Conversation, currentNodeId: string) => {
    let childrenIds;
    try {
        childrenIds = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString);
    } catch {
        console.log("WOW");
        childrenIds = [];
    }

    childrenIds.map((childId: string) => {
        const child = _getNodeById(childId, nodeList);
        if (child.nodeId !== currentNodeId) {
            children.push(child);
        }
    });
    for (let childId = 0; childId < childrenIds.length; childId++) {
        const nodeId = childrenIds[childId];
        const child = _getNodeById(nodeId, nodeList);
        children.push.apply(gatherChildren(child, nodeList, children, currentNodeId));
    }
    return children;
};

const isNonTerminalLeafNode = (node: ConvoNode) => {
    if (node.isTerminalType) return false;
    if (node.isRoot) return false;
    if (node.isMultiOptionType) return false;
    if (node.isSplitMergeType) return false;
    if (_splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString).length > 0) return false;
    return true;
};

export const recursivelyReferenceCurrentNodeInNonTerminalLeafNodes = (anabranchMergePointNodeId: string, nodeList: Conversation, rootNodeId: string) => {
    const rootNode = _getNodeById(rootNodeId, nodeList);
    const rootNodeChildIDs = _splitAndRemoveEmptyNodeChildrenString(rootNode.nodeChildrenString);

    if (rootNodeId === anabranchMergePointNodeId || rootNode.isTerminalType || rootNode.isSplitMergeType) {
        return nodeList;
    } else if (rootNode.isMultiOptionType) {
        rootNodeChildIDs.forEach((childId: string) => {
            // if multioption type - skip
            nodeList = recursivelyReferenceCurrentNodeInNonTerminalLeafNodes(anabranchMergePointNodeId, nodeList, childId);
        });
    } else {
        if (rootNode.nodeChildrenString === "") {
            // no children set, must be an unset leafnode
            if (rootNode.nodeType === "") {
                rootNode.nodeType = "ProvideInfo";
            }
            rootNode.nodeChildrenString = anabranchMergePointNodeId;
            rootNode.shouldRenderChildren = false;
        } else {
            const childIds = _splitNodeChildrenString(rootNode.nodeChildrenString);
            if (childIds.length !== 1) throw new Error("What other kind of node type is there?: " + rootNode.nodeChildrenString);

            const childNode = _getNodeById(childIds[0], nodeList);
            if (childNode.nodeId !== anabranchMergePointNodeId && childNode.nodeType === "") {
                rootNode.nodeChildrenString = anabranchMergePointNodeId;
                rootNode.shouldRenderChildren = false;
                nodeList = nodeList.filter((x: ConvoNode) => x.nodeId !== childIds[0]);
            } else {
                nodeList = recursivelyReferenceCurrentNodeInNonTerminalLeafNodes(anabranchMergePointNodeId, nodeList, childIds[0]);
            }
        }
    }

    // TODO: check that we successfully referenced the anabranch merge node.
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
    if (node.isAnabranchMergePoint) {
        resultArray.push(false);
    } else if (node.isMultiOptionType && !node.isAnabranchType) {
        const multiCheckResult = checkChildrenForUnsetChildren(node, nodeList);
        resultArray.push(multiCheckResult);
    } else {
        const childIds = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString);
        for (let childIdIndex = 0; childIdIndex < childIds.length; childIdIndex++) {
            const childId = childIds[childIdIndex];
            const results = anyMultiChoiceTypesWithUnsetChildren(childId, nodeList, resultArray);
            resultArray.push.apply(results);
        }
    }
    return resultArray;
};

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
