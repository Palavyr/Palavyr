import { Conversation, ConvoNode, NodeOption, Responses } from "@Palavyr-Types";
import { _createNewChildIDs, _getIdsToDeleteRecursively, _removeNodeByID, _truncateTheTreeAtSpecificNode } from "./_coreNodeUtils";

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
    return filteredNodeList;
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

export const changeNodeType = async (node: ConvoNode, nodeList: Conversation, optionPaths: Responses, valueOptions: Array<string>, setNodes: (nodeList: Conversation) => void, nodeOption: NodeOption) => {
    // var client = new ApiClient();

    const newNumChildren = getNewNumChildren(optionPaths);
    const { newNodeList, newChildNodeIds, childIdsToCreate } = createAndReattachNewNodes(node, nodeList, newNumChildren);

    // // reset the parentNode's children
    // if (parentNode?.isSplitMergeType && siblingIndex > 0) {
    //     const primarySibling = getPrimarySibling(parentNode, node);
    //     node.nodeChildrenString = primarySibling;
    // } else {
    //     node.nodeChildrenString = newChildNodeIds.join(",");
    // }
    node.nodeChildrenString = newChildNodeIds.join(",");

    // TODO: delete this after substantial testing. I think this is okay to go since
    // the parent node is the same node that is being passed in, and should have the updated text.
    // map old node text to new node
    // var n = nodeList.filter((x) => x.nodeId === parentNode.nodeId)[0];
    // parentNode.text = n.text;

    // is the new parent a multiOptionNodeType
    // const { data: isMultiOptionType } = await client.Conversations.CheckIfIsMultiOptionType(node.nodeType);
    node.isMultiOptionType = nodeOption.isMultiOptionType;

    // const { data: isTerminalType } = await client.Conversations.CheckIfIsTerminalType(node.nodeType);
    node.isTerminalType = nodeOption.isTerminalType;

    // const { data: isSplitMergetype } = await client.Conversations.CheckIfIsSplitMergeType(node.nodeType);
    node.isSplitMergeType = nodeOption.isSplitMergeType;

    // remove old parent node from nodelist
    const updatedNodeList = _removeNodeByID(node.nodeId, newNodeList);

    // add updated node to nodelist
    updatedNodeList.push(node);

    delete node.id;

    // set any value options
    node.valueOptions = valueOptions.join("|peg|"); // TODO: get this seperator from server

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
            optionPath: optionPaths[index],
            valueOptions: "",
            isMultiOptionType: false,
            isTerminalType: false,
            isSplitMergeType: false,
            shouldRenderChildren: computeShouldRenderChildren(node, index),
        };

        updatedNodeList.push(newNode);
    });

    setNodes(updatedNodeList);
};

const computeShouldRenderChildren = (parentNode: ConvoNode, index: number) => {
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
