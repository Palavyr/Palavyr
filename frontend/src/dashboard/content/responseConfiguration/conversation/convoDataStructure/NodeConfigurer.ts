import { NodeTypeCode, NodeTypeOptions } from "@Palavyr-Types";
import AnabranchConfigurer from "./AnabranchConfigurer";
import { IPalavyrNode } from "./Contracts";

export class NodeConfigurer {
    /**
     *
     */


    constructor() {}

    public configure(currentNode: IPalavyrNode, parentNode: IPalavyrNode | null = null, nodeTypeOptions: NodeTypeOptions) {
        if (currentNode.isRoot) {
            AnabranchConfigurer.configureAnabranchWhenRoot(currentNode);
        } else if (parentNode !== null) {
            currentNode.parentNodeReferences.addReference(parentNode);
            currentNode.addLine(parentNode.nodeId);
            AnabranchConfigurer.configureAnabranch(currentNode, parentNode, nodeTypeOptions);
            // this.configureSplitMerge(currentNode, parentNode);
        } else {
            throw new Error("Either make: current is root, or both current node and parent node are provided. ");
        }

        if (currentNode.isAnabranchMergePoint) {
            const anabranchOriginNode = AnabranchConfigurer.GetAnabranchOriginNode(currentNode);
            anabranchOriginNode.recursiveReferenceThisAnabranchOrigin(currentNode);
            currentNode.parentNodeReferences.forEach((node, index) => {
                currentNode.addLine(node.nodeId);
            });
        }
    }


    // private configureSplitMerge(currentNode: IPalavyrNode, parentNode: IPalavyrNode) {
    //     // TODO figure this out
    //     currentNode.isPalavyrSplitmergeStart = currentNode.isPalavyrSplitmergeStart;
    //     currentNode.isPalavyrSplitmergeMember = parentNode.isPalavyrSplitmergeStart || (parentNode.isPalavyrSplitmergeMember && !parentNode.isPalavyrSplitmergeEnd);
    //     currentNode.isPalavyrSplitmergePrimarybranch = currentNode.isMemberOfLeftmostBranch;
    //     currentNode.isPalavyrSplitmergeEnd = currentNode.isPalavyrSplitmergeMergePoint;

    //     if (currentNode.isPalavyrAnabranchStart) {
    //         currentNode.splitmergeContext = {
    //             ...parentNode.splitmergeContext,
    //             splitmergeOriginId: currentNode.nodeId,
    //         };
    //     }
    // }
}
