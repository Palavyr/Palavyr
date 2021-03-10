import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { Conversation, ConvoNode, NodeOption, NodeTypeOptions, ValueOptionDelimiter } from "@Palavyr-Types";
import { findIndex } from "lodash";
import { _computeShouldRenderChildren, _createAndAddNewNodes, _createNewChildIDs, _getIdsToDeleteRecursively, _getNodeById, _removeNodeByID, _replaceNodeWithUpdatedNode, _resetOptionPaths, _truncateTheTreeAtSpecificNode } from "./_coreNodeUtils";

export const getRootNode = (nodeList: Conversation) => {
    return nodeList.filter((node) => node.isRoot === true)[0];
};

export const getNewNumChildren = (optionPaths: string[]) => {
    return optionPaths.filter((x) => x !== null && x !== "").length;
};

export const checkedNodeOptionList = (nodeOptionList: NodeTypeOptions, parentNode: ConvoNode | null, siblingIndex: number) => {
    if (parentNode && parentNode.isSplitMergeType && siblingIndex > 0) {
        const compatible = nodeOptionList.filter((option: NodeOption) => option.groupName === "Provide Info" || option.groupName === "Info Collection");
        return compatible;
    } else {
        return nodeOptionList;
    }
};

export const getUnsortedChildNodes = (childrenIDs: string, nodeList: Conversation) => {
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
    const unsortedChildNodes = getUnsortedChildNodes(childrenIds, nodeList);
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

export const changeNodeType = async (previousNode: ConvoNode, nodeList: Conversation, setNodes: (nodeList: Conversation) => void, nodeOption: NodeOption) => {
    let valueOptions = previousNode.valueOptions; // if valueOptions is "", its because it was from a non-multioptionType
    if (nodeOption.isMultiOptionType && nodeOption.valueOptions.length > 0) {
        valueOptions = nodeOption.valueOptions.join(ValueOptionDelimiter);
    } else if (isNullOrUndefinedOrWhitespace(valueOptions)) {
        valueOptions = previousNode.isTerminalType ? "" : "Placeholder";
    }

    let pathOptions: string[];
    if (nodeOption.isTerminalType) {
        pathOptions = [""];
    } else if (nodeOption.isMultiOptionType) {
        if (nodeOption.pathOptions.length >= 1) {
            pathOptions = nodeOption.pathOptions; // if its a predefined yes or no
        } else {
            // use the previous node and attach
            const currentPathOptions = previousNode.valueOptions.split(ValueOptionDelimiter);
            pathOptions = currentPathOptions;
        }
    } else {
        pathOptions = ["Continue"];
    }

    // } else if (nodeOption.isMultiOptionType && nodeOption.pathOptions.length == 0) {
    //     // if the option is a multichoice or splitmerge with no predefined options. Try to take whats already there
    //     if (currentPathOptions.length == 1 && isNullOrUndefinedOrWhitespace(currentPathOptions[0])) {

    //         pathOptions = ["Placeholder"];
    //     } else {
    //         pathOptions = currentPathOptions;
    //     }
    // } else {
    //     pathOptions = ["Continue"];
    // }

    if (pathOptions === undefined) {
        throw new Error("Ill defined path options");
    }
    if (valueOptions === undefined) {
        throw new Error("Ill defined value options - cannot be undefined");
    }

    // TODO: This is kind of gross and complicates extendability since we later have to be sure not to intro any '-' in to the names. But
    // since we are taking this fromthe option, we have to deal with it as a string until we try a refactor to get it into an object form
    // so we can supply properties. ^ The option comes in from the event, which currently passes the value as a string. Can this be an object?
    previousNode.nodeType = nodeOption.value; // SelectOneFlat-sdfs-sdfs-sgs-s

    const newNumChildren = getNewNumChildren(pathOptions);
    const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(previousNode, nodeList, newNumChildren);

    const previousNodeChildrenString = previousNode.nodeChildrenString;
    previousNode.nodeChildrenString = newChildNodeIds.join(",");
    previousNode.isMultiOptionType = nodeOption.isMultiOptionType;
    previousNode.isTerminalType = nodeOption.isTerminalType;
    previousNode.isSplitMergeType = nodeOption.isSplitMergeType;
    previousNode.shouldShowMultiOption = nodeOption.shouldShowMultiOption;

    // set any value options
    previousNode.valueOptions = valueOptions;
    let updatedNodeList = _replaceNodeWithUpdatedNode(previousNode, newNodeList);
    updatedNodeList = _createAndAddNewNodes(childIdsToCreate, newChildNodeIds, previousNode, pathOptions, updatedNodeList, nodeOption.shouldShowMultiOption);

    if (newChildNodeIds.length > 0) {
        updatedNodeList = _resetOptionPaths(newChildNodeIds, updatedNodeList, pathOptions);
    } else {
        updatedNodeList = _resetOptionPaths(previousNodeChildrenString.split(","), updatedNodeList, pathOptions);
    }
    setNodes(updatedNodeList);
};

export const updateSingleOptionType = (updatedNode: ConvoNode, nodeList: Conversation, setNodes: (nodeList: Conversation) => void) => {
    const updatedNodeList = _replaceNodeWithUpdatedNode(updatedNode, nodeList);
    setNodes(updatedNodeList);
};
