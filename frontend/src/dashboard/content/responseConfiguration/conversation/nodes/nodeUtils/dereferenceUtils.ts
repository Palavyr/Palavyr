import { ConvoNode, Conversation } from "@Palavyr-Types";
import { v4 as uuid } from "uuid";
import { _getNodeById, _splitAndRemoveEmptyNodeChildrenString, _joinNodeChildrenStringArray, _createAndAddNewNodes } from "./_coreNodeUtils";

/*
 * This function will walk the tree downwards and remove all references from child node string to childIdToDereference
 */
export const recursivelyDereferenceNodeIdFromChildrenExceptWhen = (exceptNodeId: string | undefined, node: ConvoNode, nodeList: Conversation, childIdToDereference: string) => {
    const filteredChildNodeChildIds = dereferenceChildId(node, childIdToDereference, nodeList, exceptNodeId);
    if (filteredChildNodeChildIds.length > 0) {
        filteredChildNodeChildIds.forEach((id: string) => {
            const nextChildNode = _getNodeById(id, nodeList);
            recursivelyDereferenceNodeIdFromChildrenExceptWhen(exceptNodeId, nextChildNode, nodeList, childIdToDereference);
        });
    }
};

const dereferenceChildId = (node: ConvoNode, childNodeIdToDereference: string, nodeList: Conversation, exceptNodeId: string | null = null) => {
    // 1. get rid of primary signling child node ids from this node. if this node is not the leftmost parent of the node to dereference
    // 2. Then return array of filtered child nodes.
    const childrenIds = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString);
    if (node.nodeId !== exceptNodeId) {
        const newIdsToAdd: string[] = [];
        const filteredChildNodeIds = childrenIds.filter((childId: string) => childId !== childNodeIdToDereference);
        const numIdsToCreate = childrenIds.length - filteredChildNodeIds.length;

        for (let _ = 0; _ < numIdsToCreate; _++) {
            const newNodeId = uuid();
            newIdsToAdd.push(newNodeId);
        }
        _createAndAddNewNodes(newIdsToAdd, newIdsToAdd, node.areaIdentifier, ["Continue"], nodeList, false, false);
        node.nodeChildrenString = _joinNodeChildrenStringArray([...filteredChildNodeIds, ...newIdsToAdd]);
        node.shouldRenderChildren = true;
        return filteredChildNodeIds;
    }
    return childrenIds;
};

/*
 * This function will walk the tree downwards and remove all references from child node string to childIdToDereference
 */
export const recursivelyDereferenceNodeIdFromChildren = (node: ConvoNode, nodeList: Conversation, childIdToDereference: string) => {
    // recursive function.
    const filteredChildNodeChildIds = dereferenceChildId(node, childIdToDereference, nodeList);

    if (filteredChildNodeChildIds.length > 0) {
        filteredChildNodeChildIds.forEach((id: string) => {
            const nextChildNode = _getNodeById(id, nodeList);
            recursivelyDereferenceNodeIdFromChildren(nextChildNode, nodeList, childIdToDereference);
        });
    }
};
