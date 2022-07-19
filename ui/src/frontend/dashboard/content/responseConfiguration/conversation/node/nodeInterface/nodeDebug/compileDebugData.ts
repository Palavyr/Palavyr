import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { ConversationDesignerNodeResource, NodeIdentity } from "@Palavyr-Types";

export const debugDataItems = (identity: NodeIdentity) => {
    if (isNullOrUndefinedOrWhitespace(identity)) return [];
    return compileObjectData(identity);
};

export const debugNodeProperties = (node: ConversationDesignerNodeResource) => {
    if (isNullOrUndefinedOrWhitespace(node)) return [];
    return compileObjectData(node);
};

const compileObjectData = (obj: Object) => {
    const keys = Object.keys(obj);
    return keys.map((key: string) => ({ [key]: obj[key] }));
};
