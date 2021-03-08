import { Conversation, ConvoNode, NodeOption, NodeTypeOptions, Responses, ValueOptionDelimiter } from "@Palavyr-Types";
import { _computeShouldRenderChildren, _createNewChildIDs, _getIdsToDeleteRecursively, _removeNodeByID, _truncateTheTreeAtSpecificNode } from "./_coreNodeUtils";

export const getRootNode = (nodeList: Conversation) => {
    return nodeList.filter((node) => node.isRoot === true)[0];
};

export const getNodeById = (nodeId: string, nodeList: Conversation) => {
    return nodeList.filter((node: ConvoNode) => node.nodeId === nodeId)[0];
};

export const getNewNumChildren = (optionPaths: string[]) => {
    return optionPaths.filter((x) => x !== null && x !== "").length;
};

export const replaceNodeWithUpdatedNode = (nodeData: ConvoNode, nodeList: Conversation) => {
    // replace the old node with the new node in the list
    const filteredNodeList = _removeNodeByID(nodeData.nodeId, nodeList);
    filteredNodeList.push(nodeData);
    delete nodeData.id;
    return filteredNodeList;
};

export const checkedNodeOptionList = (nodeOptionList: NodeTypeOptions, parentNode: ConvoNode | null, siblingIndex: number) => {
    if (parentNode && parentNode.isSplitMergeType && siblingIndex > 0) {
        const compatible = nodeOptionList.filter((option: NodeOption) => option.groupName === "Provide Info" || option.groupName === "Info Collection");
        return compatible;
    } else {
        return nodeOptionList;
    }
};

export const getChildNodes = (childrenIDs: string, nodeList: Conversation) => {
    const ids = childrenIDs.split(",");
    return nodeList
        .filter((node) => ids.includes(node.nodeId))
        .sort(function (a, b) {
            if (a.optionPath == null || b.optionPath == null) {
                return 0;
            }
            var nameA = a.optionPath.toUpperCase(); // ignore upper and lowercase
            var nameB = b.optionPath.toUpperCase(); // ignore upper and lowercase
            if (nameA < nameB) {
                return -1;
            }
            if (nameA > nameB) {
                return 1;
            }

            // names must be equal
            return 0;
        });
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
            let nodeToTruncateFrom = getNodeById(nodeId, newNodeList);
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

export const changeNodeType = async (node: ConvoNode, nodeList: Conversation, setNodes: (nodeList: Conversation) => void, nodeOption: NodeOption) => {
    const pathOptions = nodeOption.pathOptions;
    const valueOptions = nodeOption.valueOptions;

    if (pathOptions === undefined) {
        throw new Error("Ill defined path options");
    }
    if (valueOptions === undefined) {
        throw new Error("Ill defined value options - cannot be undefined");
    }

    // TODO: This is kind of gross and complicates extendability since we later have to be sure not to intro any '-' in to the names. But
    // since we are taking this fromthe option, we have to deal with it as a string until we try a refactor to get it into an object form
    // so we can supply properties. ^ The option comes in from the event, which currently passes the value as a string. Can this be an object?
    node.nodeType = nodeOption.value; // SelectOneFlat-sdfs-sdfs-sgs-s

    const newNumChildren = getNewNumChildren(pathOptions);
    const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(node, nodeList, newNumChildren);

    node.nodeChildrenString = newChildNodeIds.join(",");
    node.isMultiOptionType = nodeOption.isMultiOptionType;
    node.isTerminalType = nodeOption.isTerminalType;
    node.isSplitMergeType = nodeOption.isSplitMergeType;

    const updatedNodeList = replaceNodeWithUpdatedNode(node, newNodeList);

    // set any value options
    node.valueOptions = valueOptions.join(ValueOptionDelimiter);

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
            optionPath: pathOptions[index],
            valueOptions: "",
            isMultiOptionType: false,
            isTerminalType: false,
            isSplitMergeType: false,
            shouldRenderChildren: _computeShouldRenderChildren(node, index),
        };

        updatedNodeList.push(newNode);
    });

    setNodes(updatedNodeList);
};
