import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { Groups, GroupNodeType } from "@Palavyr-Types";
import { sortBy } from "lodash";

export const getRootNodes = (nodeList: Groups) => {
    return nodeList.filter((node: GroupNodeType) => node.isRoot === true);
};

const optionPathGetter = (node: GroupNodeType): string => {
  return node.optionPath;
}

export const getChildNodes = (childrenIDs: string, nodeList: Groups) => {
    const ids = childrenIDs.split(",");
    return sortByPropertyAlphabetical(optionPathGetter, nodeList.filter((node) => ids.includes(node.nodeId)));
};