import { ConvoNode, Conversation, NodeOption } from "@Palavyr-Types";
import { findIndex } from "lodash";
import { createAndReattachNewNodes, _replaceNodeWithUpdatedNode } from "./commonNodeUtils";
import { _removeNodeByID } from "./_coreNodeUtils";

export const updateChildOfIsSplitMergeType = (node: ConvoNode, parentNode: ConvoNode, nodeList: Conversation, setNodes: (updatedNodeList: Conversation) => void) => {
    const primarySiblingId = getPrimarySiblingId(parentNode, node);

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

export const getPrimarySiblingId = (parentNode: ConvoNode, node: ConvoNode) => {
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

        const updatedNodeList = _replaceNodeWithUpdatedNode(node, newNodeList);
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
            };
            updatedNodeList.push(newNode);
        });
        setNodes(updatedNodeList);
    } else {
        node.shouldRenderChildren = false;
        node.nodeChildrenString = getPrimarySiblingId(parentNode, node);
        node.isMultiOptionType = false;
        node.isTerminalType = false;
        // node.isSplitMergeType = false; // don't set this when we want to experiment with multiple split merges in a row

        const updatedNodeList = _replaceNodeWithUpdatedNode(node, nodeList);
        setNodes(updatedNodeList);
    }
};
