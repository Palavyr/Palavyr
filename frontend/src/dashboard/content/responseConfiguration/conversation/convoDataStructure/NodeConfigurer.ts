import { IPalavyrNode } from "./Contracts";

export class NodeConfigurer {
    /**
     *
     */
    constructor() {}

    public configureRootNode(rootNode: IPalavyrNode) {
        this.configureAnabranchWhenRoot(rootNode);
    }

    private configureAnabranchWhenRoot(rootNode: IPalavyrNode) {
        rootNode.isPalavyrAnabranchStart = rootNode.isAnabranchType;
        rootNode.isPalavyrAnabranchMember = rootNode.isAnabranchType;
        rootNode.isPalavyrAnabranchEnd = false;
        rootNode.anabranchContext = rootNode.isAnabranchType ? { anabranchOriginId: rootNode.nodeId } : { anabranchOriginId: "" };
    }

    public configure(currentNode: IPalavyrNode, parentNode: IPalavyrNode) {
        currentNode.parentNodeReferences.addReference(parentNode);
        currentNode.addLine(parentNode.nodeId);
        this.configureAnabranch(currentNode, parentNode);
        this.configureSplitMerge(currentNode, parentNode);
    }

    private configureAnabranch(currentNode: IPalavyrNode, parentNode: IPalavyrNode) {
        // all nodes establish their own anabranch context
        // possibly update this if parent has anabranch origin node set
        currentNode.anabranchContext = {
            anabranchOriginId: "",
        };

        if (currentNode.isAnabranchType) {
            currentNode.isPalavyrAnabranchStart = true;
            currentNode.anabranchContext.anabranchOriginId = currentNode.nodeId;
        } else if (parentNode.isPalavyrAnabranchMember) {
            currentNode.anabranchContext.anabranchOriginId = parentNode.anabranchContext.anabranchOriginId;
        } else {
            currentNode.isPalavyrAnabranchStart = false;
        }

        // merge points are considered endings - and this is a switch set on the node
        currentNode.isPalavyrAnabranchEnd = currentNode.isAnabranchMergePoint;

        // the current node is an anabranch member if:
        // - the current node is anabranch type
        // - the parent is anabranch member and the current is not the end
        // - the parent is anabranch member and the current is the end and the current is anabranch type

        currentNode.isPalavyrSplitmergeMember =
            currentNode.isAnabranchType ||
            (parentNode.isPalavyrAnabranchMember && !currentNode.isPalavyrAnabranchEnd) ||
            (parentNode.isPalavyrAnabranchMember && currentNode.isPalavyrAnabranchEnd && currentNode.isPalavyrAnabranchMember);
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
