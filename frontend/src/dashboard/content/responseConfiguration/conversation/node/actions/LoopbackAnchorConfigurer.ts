import { IPalavyrNode, INodeReferences } from "../../Contracts";

class LoopbackAnchorConfigurer {
    constructor() {}

    public ConfigureLoopbackAnchorWhenRoot(currentNode: IPalavyrNode) {
        this.ConfigureLoopbackAnchor(currentNode);
    }

    public ConfigureLoopbackAnchor(currentNode: IPalavyrNode, parentNode: IPalavyrNode | null = null) {
        if (currentNode.isLoopbackAnchorType) {
            currentNode.isLoopbackStart = true;
            currentNode.isLoopbackMember = true;
            currentNode.childNodeReferences.forEach((child: IPalavyrNode, index: number) => {
                if (index > 0) {
                    this.RecursivelyApplyLoopbackContextFromThisNode(child, currentNode.nodeId);
                }
            });
        } else if (currentNode.LoopbackContextIsSet()) {
            currentNode.isLoopbackStart = false;
            currentNode.isLoopbackMember = true;
        } else if (parentNode != null && (parentNode.LoopbackContextIsSet() || parentNode.isLoopbackAnchorType)) {
            if (parentNode.isLoopbackAnchorType && parentNode.childNodeReferences.findIndexOf(currentNode) === 0) {
                currentNode.isLoopbackStart = false;
                currentNode.isLoopbackMember = false;
            } else {
                currentNode.isLoopbackStart = false;
                currentNode.isLoopbackMember = true;
            }
        } else {
            currentNode.isLoopbackStart = false;
            currentNode.isLoopbackMember = false;
        }
    }

    private RecursivelyApplyLoopbackContextFromThisNode(node: IPalavyrNode, originId: string) {
        const recurse = (childNodeReferences: INodeReferences, parent: IPalavyrNode) => {
            if (childNodeReferences.Length === 0) return;
            if (parent.nodeType === "Loopback") return;
            childNodeReferences.forEach((childNode: IPalavyrNode) => {
                this.SetLoopbackContext(childNode, originId);
                recurse(childNode.childNodeReferences, childNode);
            });
        };

        this.SetLoopbackContext(node, originId);
        recurse(node.childNodeReferences, node);
    }

    private RecursivelyRemoveLoopbackContextFromThisNode(node: IPalavyrNode) {
        const recurse = (childNodeReferences: INodeReferences) => {
            if (childNodeReferences.Length === 0) return;
            childNodeReferences.forEach((childNode: IPalavyrNode) => {
                this.ClearLoopbackContext(childNode);
                recurse(childNode.childNodeReferences);
            });
        };

        this.ClearLoopbackContext(node);
        recurse(node.childNodeReferences);
    }

    private SetLoopbackContext(currentNode: IPalavyrNode, originNodeId: string) {
        if (!currentNode.loopbackContext) {
            currentNode.loopbackContext = { loopbackOriginId: "" };
        }
        currentNode.loopbackContext.loopbackOriginId = originNodeId;
    }

    private ClearLoopbackContext(currentNode: IPalavyrNode) {
        currentNode.loopbackContext = { loopbackOriginId: "" };
    }
}

export default new LoopbackAnchorConfigurer();
