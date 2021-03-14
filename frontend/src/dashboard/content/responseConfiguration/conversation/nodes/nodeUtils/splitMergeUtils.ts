import { ConvoNode, Conversation, MostRecentSplitMerge } from "@Palavyr-Types";
import { findIndex } from "lodash";
import { _createAndAddNewNodes, _getNodeById, _getParentNode, _joinNodeChildrenStringArray, _removeNodeByID, _replaceNodeWithUpdatedNode, _splitAndRemoveEmptyNodeChildrenString, _splitNodeChildrenString } from "./_coreNodeUtils";

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

export const findMostRecentSplitMerge = (node: ConvoNode, nodeList: Conversation): MostRecentSplitMerge => {
    // returns either the most recent splitMerge type, or null
    console.group(node.text);
    console.log(node);
    console.log(node.nodeType);

    console.groupEnd();
    const SplitMerge = "SplitMerge".toUpperCase();
    const nodetype = node.nodeType;
    const optionpath = node.optionPath;
    const text = node.text;

    if (!nodeListContainsSplitmerge(nodeList)) {
        // early bail if no splitmerges
        return { isDecendentOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: [] };
    }

    if (node.nodeType.toUpperCase() === SplitMerge) {
        return { isDecendentOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: [] };
    }

    if (node.isRoot) {
        return { isDecendentOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: [] };
    }

    let found = false;
    let parentNode: ConvoNode;
    let decendentLevelFromSplitMerge = 0;
    let tempParentNode: ConvoNode | null = { ...node };
    let prevChildReference = { ...node };
    let splitMergeRootSiblingIndex: number;
    let nodeIdOfMostRecentSplitMergePrimarySibling: string;
    let orderedChildren: Conversation;
    let result: MostRecentSplitMerge = { isDecendentOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: [] };
    do {
        decendentLevelFromSplitMerge++;
        prevChildReference = { ...tempParentNode! };
        tempParentNode = _getParentNode(prevChildReference!, nodeList);
        if (tempParentNode === null) throw new Error("Orphan node detected.");
        if (tempParentNode.nodeType.toUpperCase() === SplitMerge) {
            found = true;
            parentNode = tempParentNode;
            splitMergeRootSiblingIndex = getSiblingIndex(parentNode, prevChildReference);
            nodeIdOfMostRecentSplitMergePrimarySibling = getPrimarySiblingIdFromChildNodeChildrenString(parentNode);
            orderedChildren = getorderedChildrenFromParent(parentNode, nodeList);
            result = { isDecendentOfSplitMerge: true, decendentLevelFromSplitMerge, splitMergeRootSiblingIndex, nodeIdOfMostRecentSplitMergePrimarySibling, orderedChildren };
            break;
        } else if (tempParentNode.isRoot) {
            found = true;
        } else if (decendentLevelFromSplitMerge > 200) {
            throw new Error("The tree is too deep. This cannot be allowed.");
        }
    } while (!found);
    if (decendentLevelFromSplitMerge === 3) {
        console.log("WOW");
    }
    return result;
};

export const nodeListContainsSplitmerge = (nodeList: Conversation) => {
    const nodeTypes = nodeList.map((node: ConvoNode) => node.nodeType.toUpperCase());
    return nodeTypes.includes("SplitMerge".toUpperCase());
};

export const getorderedChildrenFromParent = (parentNode: ConvoNode, nodeList: Conversation) => {
    const children = parentNode.nodeChildrenString.split(",");
    const orderedNodes: Conversation = [];
    children.forEach((c: string) => {
        let node = _getNodeById(c, nodeList);
        orderedNodes.push(node);
    });
    return orderedNodes;
};

// export const _getParentNodeYOLO = (node: ConvoNode, nodeList: Conversation) => {
//     // used when trying to find most recent splitmerge.
//     if (node.isRoot) {
//         return null;
//     }
//     const parent = nodeList.filter((n: ConvoNode) => {
//         if (isNullOrUndefinedOrWhitespace(n.nodeChildrenString)) {
//             return false;
//         }
//         let children = n.nodeChildrenString.split(",");
//         return children.includes(node.nodeId);
//     });

//     return parent[0];
// };

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

// asumes a nonprimarysibling child node to a splitmerge parent
export const rectifyMergeTypeChildReferences =  (node: ConvoNode, nodeList: Conversation, primarySiblingNodeId: string) => {
    // recursive function.
    // this is only called when changing nodetype FROM splitmerge TO non-splitmerge-nodeId
    // node: if this is run on the primary sibling, then it will deference all of the legit children of the primary sibling FROM the primary sibiling. This is a boo boo

    const filteredChildNodeChildIds = dereferencePrimarySibling(node, primarySiblingNodeId);

    if (filteredChildNodeChildIds.length > 0) {
        filteredChildNodeChildIds.forEach((id: string) => {
            const nextChildNode = _getNodeById(id, nodeList);
            rectifyMergeTypeChildReferences(nextChildNode, nodeList, primarySiblingNodeId);
        });
    }
};

const dereferencePrimarySibling = (node: ConvoNode, primarySiblingNodeId: string) => {
    // 1. get rid of primary signling childe node ids from this node.
    // 2. Then return array of filtered child nodes.
    const childrenIds = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString);
    const filteredChildNodeIds = childrenIds.filter((grandChildId: string) => grandChildId !== primarySiblingNodeId);
    node.nodeChildrenString = _joinNodeChildrenStringArray(filteredChildNodeIds);
    return filteredChildNodeIds
}