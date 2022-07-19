import { ConversationRecordUpdate, WidgetNodeResource, WidgetNodeResources } from "@Palavyr-Types";

export const MinNumeric: number = 0;

export const getRootNode = (nodeList: WidgetNodeResources): WidgetNodeResource => {
    var node = nodeList.filter(x => x.isRoot === true)[0];
    return node;
};

export const getSelectorNode = (nodeList: WidgetNodeResources): WidgetNodeResource => {
    const node = nodeList.filter(x => x.nodeComponentType === "Selection")[0];
    return node;
};

export const getOrderedChildNodes = (childrenIDs: string, nodeList: WidgetNodeResources) => {
    const ids = childrenIDs.split(",");
    const children: WidgetNodeResources = [];
    ids.forEach((id: string) => {
        const node = nodeList.filter(node => node.nodeId === id)[0]; // each ID should only refer to 1 existing node.
        if (node) children.push(node);
    });
    return children;
};

export const assembleEmailRecordData = (
    conversationId: string,
    intentId: string,
    name: string,
    email: string,
    PhoneNumber: string,
    locale: string,
    fallback: boolean = false
): Partial<ConversationRecordUpdate> => {
    return {
        ConversationId: conversationId,
        IntentId: intentId,
        Name: name,
        Email: email,
        PhoneNumber: PhoneNumber,
        Fallback: fallback,
        Locale: locale,
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
