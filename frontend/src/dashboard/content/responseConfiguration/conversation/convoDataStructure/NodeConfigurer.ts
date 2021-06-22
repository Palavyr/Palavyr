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
        rootNode.anabranchContext = rootNode.isAnabranchType ? { anabranchOriginId: rootNode.nodeId } : {anabranchOriginId: ""};
    }

    public configure(currentNode: IPalavyrNode, parentNode: IPalavyrNode) {
        currentNode.parentNodeReferences.addReference(parentNode);
        currentNode.addLine(parentNode.nodeId);
        this.configureAnabranch(currentNode, parentNode);
        this.configureSplitMerge(currentNode, parentNode);
    }

    private configureAnabranch(currentNode: IPalavyrNode, parentNode: IPalavyrNode) {
        // what kind of anabranch node am I?

        // am I the start node?
        currentNode.isPalavyrAnabranchStart = currentNode.isAnabranchType;

        // am I some member?
        currentNode.isPalavyrAnabranchMember =
            parentNode.isPalavyrAnabranchStart || (parentNode.isPalavyrAnabranchMember && !parentNode.isPalavyrAnabranchEnd) || currentNode.isPalavyrAnabranchStart || currentNode.isAnabranchMergePoint;

        // amd I an anabranch merge point?
        currentNode.isPalavyrAnabranchEnd = currentNode.isAnabranchMergePoint;

        // if I'm the start, track my node Id
        if (currentNode.isPalavyrAnabranchStart) {
            currentNode.anabranchContext = {
                ...parentNode.anabranchContext,
                anabranchOriginId: currentNode.nodeId,
            };
        } else {
            currentNode.anabranchContext = {
                ...parentNode.anabranchContext,
                anabranchOriginId: ""
            }
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
