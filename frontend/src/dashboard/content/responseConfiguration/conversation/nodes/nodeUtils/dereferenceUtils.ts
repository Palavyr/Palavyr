import { ConvoNode, Conversation } from "@Palavyr-Types";
import { _getNodeById, _splitAndRemoveEmptyNodeChildrenString, _joinNodeChildrenStringArray } from "./_coreNodeUtils";


/*
* This function will walk the tree downwards and remove all references from child node string to childIdToDereference
*/
export const recursivelyDereferenceNodeIdFromChildren =  (node: ConvoNode, nodeList: Conversation, childIdToDereference: string) => {
    // recursive function.

    const filteredChildNodeChildIds = dereferenceChildId(node, childIdToDereference);

    if (filteredChildNodeChildIds.length > 0) {
        filteredChildNodeChildIds.forEach((id: string) => {
            const nextChildNode = _getNodeById(id, nodeList);
            recursivelyDereferenceNodeIdFromChildren(nextChildNode, nodeList, childIdToDereference);
        });
    }
};

const dereferenceChildId = (node: ConvoNode, childNodeIdToDereference: string) => {
    // 1. get rid of primary signling childe node ids from this node.
    // 2. Then return array of filtered child nodes.
    const childrenIds = _splitAndRemoveEmptyNodeChildrenString(node.nodeChildrenString);
    const filteredChildNodeIds = childrenIds.filter((grandChildId: string) => grandChildId !== childNodeIdToDereference);
    node.nodeChildrenString = _joinNodeChildrenStringArray(filteredChildNodeIds);
    return filteredChildNodeIds
}