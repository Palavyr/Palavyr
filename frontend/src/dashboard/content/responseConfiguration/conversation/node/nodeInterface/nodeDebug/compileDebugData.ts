import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { ConvoNode, NodeIdentity } from "@Palavyr-Types";

export const debugDataItems = (identity: NodeIdentity) => {
    if (isNullOrUndefinedOrWhitespace(identity)) return [];
    return compileObjectData(identity);
};

export const debugNodeProperties = (node: ConvoNode) => {
    if (isNullOrUndefinedOrWhitespace(node)) return [];
    return compileObjectData(node);
};

const compileObjectData = (obj: Object) => {
    const keys = Object.keys(obj);
    return keys.map((key: string) => ({ [key]: obj[key] }));
};
