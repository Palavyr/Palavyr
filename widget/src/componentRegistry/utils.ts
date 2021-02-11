import { ConvoTableRow } from "../types";


export const getRootNode = (nodeList: Array<ConvoTableRow>): ConvoTableRow => {
    var node = nodeList.filter(x => x.isRoot === true)[0];
    return node
}

export const getChildNodes = (childrenIDs: string, nodeList: Array<ConvoTableRow>) => {
    const ids = childrenIDs.split(",");
    return nodeList.filter((node) => ids.includes(node.nodeId));
};
