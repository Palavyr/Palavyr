import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { Conversation, ConvoNode, UUID, ValueOptionDelimiter } from "@Palavyr-Types";
import { cloneDeep, findIndex } from "lodash";
import { uuid } from "uuidv4";

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

export const _getAllParentNodes = (node: ConvoNode, nodeList: Conversation): Conversation => {
    const parents = nodeList.filter((n: ConvoNode) => {
        if (isNullOrUndefinedOrWhitespace(n.nodeChildrenString)) {
            return false;
        }
        let children = _splitNodeChildrenString(n.nodeChildrenString);
        return children.includes(node.nodeId);
    });
    return parents;
};

export const _recursivelyCheckIfLeftmostChildInBranch = (node: ConvoNode, nodeList: Conversation, previousNode: ConvoNode, criteriaCallback: (node: ConvoNode) => boolean) => {
    if (criteriaCallback(node) || node.isRoot) {
        const childrenIds = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString);
        const siblingIndex = findIndex(childrenIds, (el) => el === previousNode.nodeId);
        if (siblingIndex === 0) {
            // we found the leftmost branch
            return true;
        } else {
            return false;
        }
    } else {
        const nextNodeUp = _getParentNode(node, nodeList);
        if (nextNodeUp === null) throw new Error("Tried to find parent of root node _recursivelyCheckIfLeftMostChildInBranch");
        return _recursivelyCheckIfLeftmostChildInBranch(nextNodeUp!, nodeList, node, criteriaCallback);
    }
};

export const _findLeftmostParentNode = (node: ConvoNode, nodeList: Conversation, parentNodes: Conversation, criteriaCallback: (node: ConvoNode) => boolean) => {
    if (parentNodes.length === 1) {
        return parentNodes[0];
    } else {
        for (let index = 0; index < parentNodes.length; index++) {
            const parentNode = parentNodes[index];
            const result = _recursivelyCheckIfLeftmostChildInBranch(parentNode, nodeList, node, criteriaCallback);
            if (result) {
                return parentNode;
            }
        }
        throw new Error("Orphan node detected - _findLeftmostParentNode");
    }
};

export const _getLeftMostParentNode = (node: ConvoNode, nodeList: Conversation, criteriaCallback: (node: ConvoNode) => boolean) => {
    const parentNodes = _getAllParentNodes(node, nodeList);
    const leftmostParent = _findLeftmostParentNode(node, nodeList, parentNodes, criteriaCallback);
    return leftmostParent;
};

export const _getParentNode = (node: ConvoNode, nodeList: Conversation, leftmost: boolean = false) => {
    if (node.isRoot) {
        return null;
    }

    const parents = _getAllParentNodes(node, nodeList);
    return parents[0];
};

export const _getAllParentNodeIds = (node: ConvoNode, nodeList: Conversation) => {
    if (node.isRoot) return null;
    const parentNodes = nodeList.filter((n: ConvoNode) => {
        if (isNullOrUndefinedOrWhitespace(n.nodeChildrenString)) {
            return false;
        }
        let children = _splitNodeChildrenString(n.nodeChildrenString);
        return children.includes(node.nodeId);
    });
    return parentNodes;
};

export const _createAndAddNewNodes = (childIdsToCreate: string[], newChildNodeIds: string[], areaIdentifier: string, pathOptions: string[], updatedNodeList: Conversation, shouldShowMultiOption: boolean, isDynamicTableNode: boolean) => {
    childIdsToCreate.forEach((id: string, index: number) => {
        let shift = newChildNodeIds.length - childIdsToCreate.length;
        let newNode: ConvoNode = {
            nodeId: id,
            nodeType: "",
            text: "Ask your question!",
            nodeChildrenString: "",
            isRoot: false,
            fallback: false,
            isCritical: false,
            areaIdentifier: areaIdentifier,
            optionPath: pathOptions[index + shift],
            valueOptions: "",
            isMultiOptionType: false,
            isTerminalType: false,
            isSplitMergeType: false,
            shouldRenderChildren: true,
            shouldShowMultiOption: shouldShowMultiOption,
            isAnabranchMergePoint: false,
            isAnabranchType: false,
            nodeComponentType: "",
            isDynamicTableNode: isDynamicTableNode,
            resolveOrder: 0,
            dynamicType: "",
        };

        updatedNodeList.push(newNode);
    });
    return updatedNodeList;
};
