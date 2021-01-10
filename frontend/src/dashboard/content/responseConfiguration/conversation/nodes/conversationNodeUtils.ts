import { Conversation, ConvoNode, Responses } from "@Palavyr-Types";
import { cloneDeep, intersectionWith } from "lodash";
import { v4 as uuid } from "uuid";
import { ApiClient } from "@api-client/Client";

export type UUID = string;

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

export const getRootNode = (nodeList: Conversation) => {
    return nodeList.filter((node) => node.isRoot === true)[0];
};

export const removeNodes = (nodeList: Conversation, nodeId: string, setNodes: (nodeList: Conversation) => void) => {
    // removes the node by nodeId
    let currentMeta = nodeList.filter((node: ConvoNode) => node.nodeId === nodeId).pop() as ConvoNode;
    if (currentMeta.isRoot) {
        return false;
    } else {
        nodeList = nodeList.filter((node) => node.nodeId !== nodeId);
        setNodes(cloneDeep(nodeList));
    }
};

export const createNewChildIDs = (count: number): Array<UUID> => {
    let idArray: Array<string> = [];
    for (var i = 0; i < count; i++) {
        let newID = uuid();
        idArray.push(newID);
    }
    return idArray;
};

export const removeNodeByID = (Id: string, nodeList: Conversation) => {
    return nodeList.filter((node: ConvoNode) => node.nodeId !== Id);
};

const getIdsToDeleteRecursively = (nodeList: Conversation, topNode: ConvoNode): string => {
    var childRefs = topNode.nodeChildrenString.split(",");
    var childNodes = nodeList.filter((node) => childRefs.includes(node.nodeId));

    var nextRefs: Array<string> = [];
    childNodes.forEach((node) => {
        if (node.nodeChildrenString.length > 0) {
            var refs = getIdsToDeleteRecursively(nodeList, node);
            nextRefs.push(refs);
        }
    });
    childRefs.push(...nextRefs);
    return childRefs.join(",");
};

export const addNodes = async (parentNode: ConvoNode, nodeList: Conversation, newIDs: Array<string>, optionPaths: Responses, valueOptions: Array<string>, setNodes: (nodeList: Conversation) => void) => {
    var client = new ApiClient();

    // TODO: we should never have fewer than 1 node in the nodeList - but we can assess passing through the area Id or using redux later on
    var areaIdentifier = nodeList[0].areaIdentifier;
    var treeIds = getIdsToDeleteRecursively(nodeList, parentNode);
    var childIdsToDelete = treeIds.split(",");
    var idsToDelete = [parentNode.nodeId, ...childIdsToDelete].filter((x) => x !== "");

    childIdsToDelete.forEach((nodeId) => {
        nodeList = removeNodeByID(nodeId, nodeList);
    });

    // reset the parentNode's children
    parentNode.nodeChildrenString = newIDs.join(",");

    // map old node text to new node
    var n = nodeList.filter((x) => x.nodeId === parentNode.nodeId)[0];
    parentNode.text = n.text;

    // is the new parent a multiOptionNodeType
    const { data: isMultiOptionType } = await client.Conversations.CheckIfIsMultiOptionType(parentNode.nodeType);
    parentNode.isMultiOptionType = isMultiOptionType;

    const {data: isTerminalType } = await client.Conversations.CheckIfIsTerminalType(parentNode.nodeType);
    parentNode.isTerminalType = isTerminalType;

    // remove old parent node from nodelist
    nodeList = removeNodeByID(parentNode.nodeId, nodeList);

    // add updated node to nodelist
    nodeList.push(parentNode);

    delete parentNode.id;

    // set any value options
    parentNode.valueOptions = valueOptions.join("|peg|"); // TODO: get this seperator from server

    var transactions: Conversation = [parentNode];
    newIDs.forEach((id, index) => {
        let newNode: ConvoNode = {
            nodeId: id, // replace with uuid
            nodeType: "", // default
            text: "Ask your question!",
            nodeChildrenString: "",
            isRoot: false,
            fallback: false,
            isCritical: false,
            areaIdentifier: parentNode.areaIdentifier,
            optionPath: optionPaths[index],
            valueOptions: "",
            isMultiOptionType: false,
            isTerminalType: false
        };
        transactions.push(newNode);
        nodeList.push(newNode);
    });
    const { data } = await client.Conversations.ModifyConversation(transactions, areaIdentifier, idsToDelete);
    setNodes([...cloneDeep(nodeList)]);
};

export const updateNodeList = (nodeList: Conversation, newNode: ConvoNode) => {
    var filteredList = nodeList.filter((x) => x.nodeId !== newNode.nodeId); // does this do anything...? it remove the newNode, and then adds it back in?
    return [...filteredList, newNode];
};
