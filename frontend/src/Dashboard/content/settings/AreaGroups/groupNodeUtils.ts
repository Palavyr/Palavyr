import { Groups, GroupNodeType } from "@Palavyr-Types";

export const getRootNodes = (nodeList: Groups) => {
    return nodeList.filter((node: GroupNodeType) => node.isRoot === true);
};


export const getChildNodes = (childrenIDs: string, nodeList: Groups) => {
    const ids = childrenIDs.split(",");
    return nodeList.filter((node) => ids.includes(node.nodeId)).sort(function(a, b) {
        if (a.optionPath == null || b.optionPath == null) {
            return 0
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