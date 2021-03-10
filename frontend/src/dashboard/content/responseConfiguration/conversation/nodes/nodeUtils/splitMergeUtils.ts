import { ConvoNode, Conversation, NodeOption, MostRecentSplitMerge } from "@Palavyr-Types";
import { findIndex } from "lodash";
import { createAndReattachNewNodes } from "./commonNodeUtils";
import { _createAndAddNewNodes, _getNodeById, _getParentNode, _removeNodeByID, _replaceNodeWithUpdatedNode } from "./_coreNodeUtils";

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

export const changeChildOfSplitMergeType = (node: ConvoNode, nodeList, parentNode: ConvoNode, setNodes: (nodeList: Conversation) => void, nodeOption: NodeOption) => {
    // TODO: This is kind of gross and complicates extendability since we later have to be sure not to intro any '-' in to the names. But
    // since we are taking this fromthe option, we have to deal with it as a string until we try a refactor to get it into an object form
    // so we can supply properties. ^ The option comes in from the event, which currently passes the value as a string. Can this be an object?
    node.nodeType = nodeOption.value; // SelectOneFlat-sdfs-sdfs-sgs-s

    const siblingIndex = getSiblingIndex(parentNode, node);
    if (siblingIndex === 0) {
        // if this is the primary sibling
        node.shouldRenderChildren = true;

        const newNumChildren = nodeOption.pathOptions.length;
        const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(node, nodeList, newNumChildren);

        node.nodeChildrenString = newChildNodeIds.join(",");

        let updatedNodeList = _replaceNodeWithUpdatedNode(node, newNodeList);

        // updatedNodeList = _createAndAddNewNodes(childIdsToCreate, newChildNodeIds, node, pathOpt)

        childIdsToCreate.forEach((id: string, index: number) => {
            let newNode: ConvoNode = {
                nodeId: id, // replace with uuid
                nodeType: "", // default
                text: "Ask your question!",
                nodeChildrenString: "",
                isRoot: false,
                fallback: false,
                isCritical: false,
                areaIdentifier: node.areaIdentifier,
                optionPath: nodeOption.pathOptions[index],
                valueOptions: "",
                isMultiOptionType: false,
                isTerminalType: false,
                isSplitMergeType: false,
                shouldRenderChildren: true,
                shouldShowMultiOption: false
            };
            updatedNodeList.push(newNode);
        });
        setNodes(updatedNodeList);
    } else {
        node.shouldRenderChildren = false;
        node.nodeChildrenString = getPrimarySiblingIdFromParent(parentNode);
        node.isMultiOptionType = false;
        node.isTerminalType = false;
        // node.isSplitMergeType = false; // don't set this when we want to experiment with multiple split merges in a row

        const updatedNodeList = _replaceNodeWithUpdatedNode(node, nodeList);
        setNodes(updatedNodeList);
    }
};

export const findMostRecentSplitMerge = (node: ConvoNode, nodeList: Conversation): MostRecentSplitMerge => {
    // returns either the most recent splitMerge type, or null
    const SplitMerge = "SplitMerge".toUpperCase();

    if (!nodeListContainsSplitmerge(nodeList)) {// early bail if no splitmerges
        return { isChildOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: []}
    }

    if (node.nodeType.toUpperCase() === SplitMerge) {
        return { isChildOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: []};
    }

    if (node.isRoot) {
        return { isChildOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: []};
    }

    let found = false;
    let parentNode: ConvoNode;
    let decendentLevelFromSplitMerge = 0;
    let tempParentNode: ConvoNode | null = { ...node };
    let prevChildReference = { ...node };
    let splitMergeRootSiblingIndex: number;
    let nodeIdOfMostRecentSplitMergePrimarySibling: string;
    let orderedChildren: Conversation;
    let result: MostRecentSplitMerge = { isChildOfSplitMerge: false, decendentLevelFromSplitMerge: 0, splitMergeRootSiblingIndex: 0, nodeIdOfMostRecentSplitMergePrimarySibling: "", orderedChildren: []};
    do {
        decendentLevelFromSplitMerge++;
        prevChildReference = { ...tempParentNode! };
        tempParentNode = _getParentNode(prevChildReference!, nodeList);
        if (tempParentNode === null) throw new Error("Orphan node detected.");
        if (tempParentNode.nodeType.toUpperCase() === SplitMerge) {
            found = true;
            parentNode = tempParentNode;
            splitMergeRootSiblingIndex = getSiblingIndex(parentNode, prevChildReference);
            nodeIdOfMostRecentSplitMergePrimarySibling = getPrimarySiblingIdFromParent(parentNode);
            orderedChildren = getorderedChildrenFromParent(parentNode, nodeList);
            result = { isChildOfSplitMerge: true, decendentLevelFromSplitMerge, splitMergeRootSiblingIndex, nodeIdOfMostRecentSplitMergePrimarySibling, orderedChildren};
            break;
        } else if (tempParentNode.isRoot) {
            found = true;
        } else if (decendentLevelFromSplitMerge > 1000) {
            found = true;
        }
    } while (!found);

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