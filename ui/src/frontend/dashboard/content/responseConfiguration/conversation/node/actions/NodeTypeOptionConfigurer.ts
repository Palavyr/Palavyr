import { NodeTypeOptionResource, NodeTypeCodeEnum, NodeTypeOptionResources } from "@Palavyr-Types";
import { INodeReferences, IPalavyrNode } from "@Palavyr-Types";

class NodeTypeOptionConfigurer {
    constructor() {}

    public ConfigureNodeTypeOptions(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        let options = nodeTypeOptions;
        if (!currentNode.isPalavyrAnabranchStart && currentNode.isPalavyrAnabranchMember) {
            options = this.filterUnallowedNodeOptions([NodeTypeCodeEnum.VI, NodeTypeCodeEnum.VII, NodeTypeCodeEnum.VIII], options);
        }

        if (currentNode.childNodeReferences.references.map((x) => x.nodeType).includes("Anabranch")) {
            options = this.filterUnallowedNodeOptions([NodeTypeCodeEnum.VI], options);
        }

        if (currentNode.isLoopbackMember && !currentNode.isLoopbackStart) {
            options = this.filterUnallowedNodeOptions([NodeTypeCodeEnum.VII], options);
        } else {
            options = this.filterUnallowedNodeOptions([NodeTypeCodeEnum.VIII], options);
        }

        if (currentNode.childNodeReferences.containsNodeType("Loopback")) {
            options = this.filterUnallowedNodeOptions([NodeTypeCodeEnum.VI], options);
        }

        if (currentNode.parentNodeReferences.Length === 1 && currentNode.parentNodeReferences.retrieveLeftmostReference()?.isPalavyrAnabranchStart && currentNode.isLocked) {
            options = this.filterUnallowedNodeOptions([NodeTypeCodeEnum.VIII], options);
        }

        if (currentNode.parentNodeReferences.Length === 1 && currentNode.parentNodeReferences.retrieveLeftmostReference()?.isLoopbackAnchorType) {
            options = this.filterUnallowedNodeOptions([NodeTypeCodeEnum.VII], options);
        }

        if (currentNode.childNodeReferences.containsNodeType("LoopbackAnchor")) {
            options = this.filterUnallowedNodeOptions([NodeTypeCodeEnum.VII], options);
        }

        // if we are the primary anabranch branch and a anabranch member...
        if (currentNode.isPalavyrAnabranchMember && currentNode.anabranchContext && currentNode.anabranchContext.leftmostAnabranch && currentNode.anabranchContext.leftmostAnabranch === true) {
            options = this.filterUnallowedNodeOptions([NodeTypeCodeEnum.I], options);
        }

        return options;
    }

    public RecursivelyReconfigureNodeTypeOptions(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
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

    public filterUnallowedNodeOptions(forbiddenOptions: Array<NodeTypeCodeEnum>, nodeTypeOptions: NodeTypeOptionResources) {
        const filteredNodeTypeOptions = nodeTypeOptions.filter((option: NodeTypeOptionResource) => {
            return !forbiddenOptions.includes(option.nodeTypeCodeEnum);
        });
        return filteredNodeTypeOptions;
    }
}

export default new NodeTypeOptionConfigurer();
