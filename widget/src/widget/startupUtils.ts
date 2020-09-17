import { NodeTypes } from "../componentRegistry";
import { ConvoTableRow } from "../types";


export const traverseTheTreeFromTop = (nodeList: Array<ConvoTableRow>, node: ConvoTableRow) => {
    var count = 0
    if (node.nodeType === NodeTypes.TooComplicated || node.nodeType === NodeTypes.EndingSequence) {
        return count + 1
    } else {
        var children = node.nodeChildrenString.split(",");
        for (var i: number = 0; i < children.length; i++) {
            var childNode = nodeList.filter(x => x.nodeId == children[i])
            if (childNode.length > 0) {
                count = count + traverseTheTreeFromTop(nodeList, childNode[0])
            }
        }
        return count
    }
}
