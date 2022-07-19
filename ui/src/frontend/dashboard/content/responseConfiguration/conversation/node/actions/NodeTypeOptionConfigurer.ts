import { NodeTypeOptionResource, NodeTypeCode, NodeTypeOptions } from "@Palavyr-Types";
import { INodeReferences, IPalavyrNode } from "@Palavyr-Types";

class NodeTypeOptionConfigurer {
    constructor() {}

    public ConfigureNodeTypeOptions(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        let options = nodeTypeOptions;
        if (!currentNode.isPalavyrAnabranchStart && currentNode.isPalavyrAnabranchMember) {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VI, NodeTypeCode.VII, NodeTypeCode.VIII], options);
        }

        if (currentNode.childNodeReferences.references.map((x) => x.nodeType).includes("Anabranch")) {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VI], options);
        }

        if (currentNode.isLoopbackMember && !currentNode.isLoopbackStart) {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VII], options);
        } else {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VIII], options);
        }

        if (currentNode.childNodeReferences.containsNodeType("Loopback")) {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VI], options);
        }

        if (currentNode.parentNodeReferences.Length === 1 && currentNode.parentNodeReferences.retrieveLeftmostReference()?.isPalavyrAnabranchStart && currentNode.isLocked) {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VIII], options);
        }

        if (currentNode.parentNodeReferences.Length === 1 && currentNode.parentNodeReferences.retrieveLeftmostReference()?.isLoopbackAnchorType) {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VII], options);
        }

        if (currentNode.childNodeReferences.containsNodeType("LoopbackAnchor")) {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VII], options);
        }

        // if we are the primary anabranch branch and a anabranch member...
        if (currentNode.isPalavyrAnabranchMember && currentNode.anabranchContext && currentNode.anabranchContext.leftmostAnabranch && currentNode.anabranchContext.leftmostAnabranch === true) {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.I], options);
        }

        return options;
    }

    public RecursivelyReconfigureNodeTypeOptions(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        this.ConfigureNodeTypeOptions(currentNode, nodeTypeOptions);

        const recurse = (childNodeReferences: INodeReferences) => {
            if (childNodeReferences.Length === 0) return;
            childNodeReferences.forEach((node: IPalavyrNode) => {
                this.ConfigureNodeTypeOptions(node, nodeTypeOptions);
                if (node.nodeType === "Loopback") {
                    return;
                }
                recurse(node.childNodeReferences);
            });
        };

        recurse(currentNode.childNodeReferences);
    }

    public filterUnallowedNodeOptions(forbiddenOptions: Array<NodeTypeCode>, nodeTypeOptions: NodeTypeOptions) {
        const filteredNodeTypeOptions = nodeTypeOptions.filter((option: NodeTypeOptionResource) => {
            return !forbiddenOptions.includes(option.nodeTypeCode);
        });
        return filteredNodeTypeOptions;
    }
}

export default new NodeTypeOptionConfigurer();
