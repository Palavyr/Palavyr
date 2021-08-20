import { ConversationRecordUpdate, WidgetNodeResource, WidgetNodes } from "@Palavyr-Types";

export const MinNumeric: number = 0;

export const getRootNode = (nodeList: WidgetNodes): WidgetNodeResource => {
    var node = nodeList.filter(x => x.isRoot === true)[0];
    return node;
};

export const getOrderedChildNodes = (childrenIDs: string, nodeList: WidgetNodes) => {
    const ids = childrenIDs.split(",");
    const children: WidgetNodes = [];
    ids.forEach((id: string) => {
        const node = nodeList.filter(node => node.nodeId === id)[0]; // each ID should only refer to 1 existing node.
        children.push(node);
    });
    return children;
};

export const assembleEmailRecordData = (conversationId: string, areaIdentifier: string, name: string, email: string, PhoneNumber: string, locale: string, fallback: boolean = false): Partial<ConversationRecordUpdate> => {
    return {
        ConversationId: conversationId,
        AreaIdentifier: areaIdentifier,
        Name: name,
        Email: email,
        PhoneNumber: PhoneNumber,
        Fallback: fallback,
        Locale: locale
    };
};

export const extractDynamicTypeGuid = (dynamicType: string) => {
    const parts = dynamicType.split("-");
    const guidParts = parts.slice(1);
    const guid = guidParts.join("-");
    return guid;
};

export const parseNumericResponse = (newValue: string): string => {
    const intValue = parseInt(newValue);
    return intValue < MinNumeric ? MinNumeric.toString() : intValue.toString();
};
