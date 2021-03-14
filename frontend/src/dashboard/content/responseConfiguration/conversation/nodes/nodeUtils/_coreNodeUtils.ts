import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { Conversation, ConvoNode, UUID, ValueOptionDelimiter } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
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

export const _truncateTheTreeAtSpecificNode = (node: ConvoNode, nodeList: Conversation): Conversation => {
    const treeIds = _getIdsToDeleteRecursively(nodeList, node);
    const childIdsToDelete = treeIds.split(",");

    let truncatedNodeList: Conversation = cloneDeep(nodeList);
    childIdsToDelete.forEach((nodeId) => {
        truncatedNodeList = _removeNodeByID(nodeId, truncatedNodeList);
    });
    return truncatedNodeList;
};

export const _removeNodeByID = (Id: string, nodeList: Conversation): Conversation => {
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
    // pathOptions can be [""]
    let rectifiedNodeList = [...nodeList];
    const rectifiedPathOptions = pathOptions.map((x: string) => (isNullOrUndefinedOrWhitespace(x) ? "Placeholder" : x));
    for (let i: number = 0; i < newChildNodeIds.length; i++) {
        let nodeId = newChildNodeIds[i];
        let node = _getNodeById(nodeId, nodeList);
        node.optionPath = rectifiedPathOptions[i];
        rectifiedNodeList = _replaceNodeWithUpdatedNode(node, rectifiedNodeList);
    }
    return rectifiedNodeList;
};

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

export const _getParentNode = (node: ConvoNode, nodeList: Conversation) => {
    if (node.isRoot) {
        return null;
    }
    const parent = nodeList.filter((n: ConvoNode) => {
        if (isNullOrUndefinedOrWhitespace(n.nodeChildrenString)) {
            return false;
        }
        let children = _splitNodeChildrenString(n.nodeChildrenString);
        return children.includes(node.nodeId);
    });

    // if (parent.length != 1) {
    //     const splitmergeparent = parent.filter((node: ConvoNode) => node.isSplitMergeType);
    //     if (splitmergeparent.length > 1) {
    //         throw new Error("Currently we only support multiple parents when the primary parent is a splitmerge type node.");
    //     } else {
    //         return splitmergeparent[0];
    //     }
    // }
    return parent[0];
};

export const _getAllParentNodeIds = (node: ConvoNode, nodeList: Conversation) => {
    if (node.isRoot) return null;
    const parentNodes = nodeList.filter((n: ConvoNode) => {
        if (isNullOrUndefinedOrWhitespace(n.nodeChildrenString)) {
            return false;
        }
        let children = _splitNodeChildrenString(n.nodeChildrenString);
        return children.includes(node.nodeId);
    })
    return parentNodes;
}

export const _createAndAddNewNodes = (childIdsToCreate: string[], newChildNodeIds: string[], node: ConvoNode, pathOptions: string[], updatedNodeList: Conversation, shouldShowMultiOption: boolean) => {
    childIdsToCreate.forEach((id: string, index: number) => {
        let shift = newChildNodeIds.length - childIdsToCreate.length;
        let newNode: ConvoNode = {
            nodeId: id, // replace with uuid
            nodeType: "", // default
            text: "Ask your question!",
            nodeChildrenString: "",
            isRoot: false,
            fallback: false,
            isCritical: false,
            areaIdentifier: node.areaIdentifier,
            optionPath: pathOptions[index + shift],
            valueOptions: "",
            isMultiOptionType: false,
            isTerminalType: false,
            isSplitMergeType: false,
            shouldRenderChildren: true,
            shouldShowMultiOption: shouldShowMultiOption,
            isAnabranchMergePoint: false,
            isAnabranchType: false,
        };

        updatedNodeList.push(newNode);
    });
    return updatedNodeList;
};

export const _splitNodeChildrenString = (nodeChildrenString: string) => {
    return nodeChildrenString.split(",");
};

export const _joinNodeChildrenStringArray = (nodeChildrenStrings: string[]) => {
    return nodeChildrenStrings.join(",");
};

export const _splitAndRemoveEmptyNodeChildrenString = (nodeChildrenString: string) => {
    const childrenArray = _splitNodeChildrenString(nodeChildrenString);
    return childrenArray.filter((childstring: string) => !isNullOrUndefinedOrWhitespace(childstring));
};

export const _nodeListContainsNodeType = (nodeList: Conversation, nodeType: string) => {
    const nodeTypes = nodeList.map((node: ConvoNode) => node.nodeType.toUpperCase());
    return nodeTypes.includes(nodeType.toUpperCase());
};

export const _joinValueOptionArray = (valueOptionArray: string[]) => {
    return valueOptionArray.join(ValueOptionDelimiter);
};

export const _splitValueOptionString = (valueOptionString: string) => {
    return valueOptionString.split(ValueOptionDelimiter);
};
