import { CompleteConverationDetails, WidgetNodeResource, WidgetNodes } from "@Palavyr-Types";

export const getRootNode = (nodeList: WidgetNodes): WidgetNodeResource => {
    var node = nodeList.filter(x => x.isRoot === true)[0];
    return node
}

export const getChildNodes = (childrenIDs: string, nodeList: WidgetNodes) => {
    const ids = childrenIDs.split(",");
    return nodeList.filter((node) => ids.includes(node.nodeId));
};

export const assembleCompletedConvo = (conversationId: string, areaIdentifier: string, name: string, email: string, PhoneNumber: string): CompleteConverationDetails => {
    return {
        ConversationId: conversationId,
        AreaIdentifier: areaIdentifier,
        Name: name,
        Email: email,
        PhoneNumber: PhoneNumber,
    };
};
