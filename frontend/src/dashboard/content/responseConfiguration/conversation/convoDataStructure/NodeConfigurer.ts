import { NodeTypeCode, NodeTypeOptions } from "@Palavyr-Types";
import { IPalavyrNode } from "./Contracts";
import NodeTypeOptionConfigurer from "./NodeTypeOptionConfigurer";

export class NodeConfigurer {
    /**
     *
     */
    constructor() {}

    public configure(currentNode: IPalavyrNode, parentNode: IPalavyrNode | null = null, nodeTypeOptions: NodeTypeOptions) {
        if (currentNode.isRoot) {
            this.configureAnabranchWhenRoot(currentNode);
        } else if (parentNode !== null) {
            currentNode.parentNodeReferences.addReference(parentNode);
            currentNode.addLine(parentNode.nodeId);
            this.configureAnabranch(currentNode, parentNode, nodeTypeOptions);
            // this.configureSplitMerge(currentNode, parentNode);
        } else {
            throw new Error("Either make: current is root, or both current node and parent node are provided. ");
        }
    }

    private configureAnabranchWhenRoot(rootNode: IPalavyrNode) {
        rootNode.isPalavyrAnabranchStart = rootNode.isAnabranchType;
        rootNode.isPalavyrAnabranchMember = rootNode.isAnabranchType;
        rootNode.isPalavyrAnabranchEnd = false;
        rootNode.anabranchContext = rootNode.isAnabranchType ? { anabranchOriginId: rootNode.nodeId, leftmostAnabranch: false } : { anabranchOriginId: "", leftmostAnabranch: false };
    }

    private configureAnabranch(currentNode: IPalavyrNode, parentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        // all nodes establish their own anabranch context
        // possibly update this if parent has anabranch origin node set
        currentNode.anabranchContext = {
            anabranchOriginId: "",
            leftmostAnabranch: false,
        };

        // the current node is an anabranch member if:
        // - the current node is anabranch type
        // - the parent is anabranch member and the current is not the end
        // - the parent is anabranch member and the current is the end and the current is anabranch type

        currentNode.isPalavyrAnabranchMember =
            currentNode.isAnabranchType ||
            (parentNode.isPalavyrAnabranchMember && !currentNode.isPalavyrAnabranchEnd) ||
            (parentNode.isPalavyrAnabranchMember && currentNode.isPalavyrAnabranchEnd && currentNode.isPalavyrAnabranchMember);

        if (currentNode.isAnabranchType) {
            currentNode.isPalavyrAnabranchStart = true;
            currentNode.anabranchContext.anabranchOriginId = currentNode.nodeId;
        } else if (parentNode.isPalavyrAnabranchMember) {
            currentNode.anabranchContext.anabranchOriginId = parentNode.anabranchContext.anabranchOriginId;
            currentNode.isPalavyrAnabranchStart = false;
        } else {
            currentNode.isPalavyrAnabranchStart = false;
        }

        if (parentNode.isPalavyrAnabranchStart) {
            // then I need to determine if this is the leftmost child if I need to reach in to parent child references and see if this node is the
            const index = parentNode.childNodeReferences.findIndexOf(currentNode);
            const isLeftMost = index === 0;
            currentNode.anabranchContext.leftmostAnabranch = isLeftMost;
        } else if (currentNode.isPalavyrAnabranchMember) {
            // if parents.childnoderefs is more than one, then if this is not leftmost - change
            // otherwise original code
            if (parentNode.childNodeReferences.Length > 1) {
                const index = parentNode.childNodeReferences.findIndexOf(currentNode);
                const isLeftmost = index === 0;
                currentNode.anabranchContext.leftmostAnabranch = isLeftmost;
            } else {
                const parentLeftmost = parentNode.anabranchContext.leftmostAnabranch;
                currentNode.anabranchContext.leftmostAnabranch = parentLeftmost;
            }
        } else if (currentNode.isPalavyrAnabranchEnd) {
        }

        // merge points are considered endings - and this is a switch set on the node
        currentNode.isPalavyrAnabranchEnd = currentNode.isAnabranchMergePoint;

        if (currentNode.isPalavyrAnabranchMember) {
            const notAllowedInsideAnabranch = [NodeTypeCode.VI, NodeTypeCode.VII];
            if (currentNode.anabranchContext.leftmostAnabranch) {
                notAllowedInsideAnabranch.push(NodeTypeCode.IV);
                notAllowedInsideAnabranch.push(NodeTypeCode.V);
            }
            const options = NodeTypeOptionConfigurer.filterUnallowedNodeOptions(notAllowedInsideAnabranch, nodeTypeOptions);
            currentNode.setNodeTypeOptions(options);
        } else {
            const options = NodeTypeOptionConfigurer.filterUnallowedNodeOptions([], nodeTypeOptions);
            currentNode.setNodeTypeOptions(options);
        }
    }

    private configureSplitMerge(currentNode: IPalavyrNode, parentNode: IPalavyrNode) {
        // TODO figure this out
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
