import { Conversation, ConvoNode, UUID } from "@Palavyr-Types";
import { uuid } from "uuidv4";

export const _getIdsToDeleteRecursively = (nodeList: Conversation, topNode: ConvoNode): string => {
    var childRefs = topNode.nodeChildrenString.split(",");
    var childNodes = nodeList.filter((node) => childRefs.includes(node.nodeId));

    var nextRefs: string[] = [];
    childNodes.forEach((node) => {
        if (node.nodeChildrenString.length > 0) {
            var refs = _getIdsToDeleteRecursively(nodeList, node);
            nextRefs.push(refs);
        }
    });
    childRefs.push(...nextRefs);
    return childRefs.join(",");
};

export const _truncateTheTreeAtSpecificNode = (node: ConvoNode, nodeList: Conversation) => {
    const treeIds = _getIdsToDeleteRecursively(nodeList, node);
    const childIdsToDelete = treeIds.split(",");

    let truncatedNodeList = [...nodeList];
    childIdsToDelete.forEach((nodeId) => {
        truncatedNodeList = _removeNodeByID(nodeId, truncatedNodeList);
    });
    return truncatedNodeList;
};

export const _removeNodeByID = (Id: string, nodeList: Conversation) => {
    return nodeList.filter((node: ConvoNode) => node.nodeId !== Id);
};

export const _createNewChildIDs = (count: number): UUID[] => {
    let idArray: Array<string> = [];
    for (var i = 0; i < count; i++) {
        let newID = uuid();
        idArray.push(newID);
    }
    return idArray;
};

export const _computeShouldRenderChildren = (parentNode: ConvoNode, index: number) => {
    if (parentNode.isSplitMergeType) {
        if (index === 0) {
            return true;
        } else {
            return false;
        }
    } else {
        return true;
    }
};

export const _resetOptionPaths = (newChildNodeIds: string[], nodeList: Conversation, pathOptions: string[]) => {

    let rectifiedNodeList = [...nodeList];

    for (let i: number = 0; i < pathOptions.length; i++) {
        let nodeId = newChildNodeIds[i];
        let node = _getNodeById(nodeId, nodeList);
        node.optionPath = pathOptions[i]
        rectifiedNodeList = _replaceNodeWithUpdatedNode(node, rectifiedNodeList);
    }
    return rectifiedNodeList;
}

export const _getNodeById = (nodeId: string, nodeList: Conversation) => {
    return nodeList.filter((node: ConvoNode) => node.nodeId === nodeId)[0];
};

export const _replaceNodeWithUpdatedNode = (nodeData: ConvoNode, nodeList: Conversation) => {
    // replace the old node with the new node in the list
    const filteredNodeList = _removeNodeByID(nodeData.nodeId, nodeList);
    filteredNodeList.push(nodeData);
    delete nodeData.id;
    return filteredNodeList;
};
