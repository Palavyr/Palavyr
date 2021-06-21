import { IPalavyrNode } from "./Contracts";

export class NodeConfigurer {
    /**
     *
     */
    constructor() {}

    public configure(currentNode: IPalavyrNode, parentNode: IPalavyrNode) {
        currentNode.parentNodeReferences.addReference(parentNode);
        currentNode.addLine(parentNode.nodeId);
        this.configureAnabranch(currentNode, parentNode);
        this.configureSplitMerge(currentNode, parentNode);
    }

    private configureAnabranch(currentNode: IPalavyrNode, parentNode: IPalavyrNode) {
        currentNode.isPalavyrAnabranchStart = currentNode.isPalavyrAnabranchStart;
        currentNode.isPalavyrAnabranchMember =
            parentNode.isPalavyrAnabranchStart || (parentNode.isPalavyrAnabranchMember && !parentNode.isPalavyrAnabranchEnd) || currentNode.isPalavyrAnabranchStart || currentNode.isAnabranchMergePoint;
        currentNode.isPalavyrAnabranchEnd = currentNode.isAnabranchMergePoint;

        if (currentNode.isPalavyrAnabranchStart) {
            currentNode.anabranchContext = {
                ...parentNode.anabranchContext,
                anabranchOriginId: currentNode.nodeId,
            };
        }
    }

    private configureSplitMerge(currentNode: IPalavyrNode, parentNode: IPalavyrNode) {
        currentNode.isPalavyrSplitmergeStart = currentNode.isPalavyrSplitmergeStart;
        currentNode.isPalavyrSplitmergeMember = parentNode.isPalavyrSplitmergeStart || (parentNode.isPalavyrSplitmergeMember && !parentNode.isPalavyrSplitmergeEnd);
        currentNode.isPalavyrSplitmergePrimarybranch = currentNode.isMemberOfLeftmostBranch;
        currentNode.isPalavyrSplitmergeEnd = currentNode.isPalavyrSplitmergeMergePoint;

        if (currentNode.isPalavyrAnabranchStart) {
            currentNode.splitmergeContext = {
                ...parentNode.splitmergeContext,
                splitmergeOriginId: currentNode.nodeId,
            };
        }
    }
}
