import { NodeOption, NodeTypeCode, NodeTypeOptions } from "@Palavyr-Types";
import { INodeReferences, IPalavyrNode } from "./Contracts";

class NodeTypeOptionConfigurer {
    constructor() {}

    public ConfigureNodeTypeOptions(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        let options = nodeTypeOptions;
        if (currentNode.isPalavyrAnabranchMember) {
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VI, NodeTypeCode.VII], options);
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
            options = this.filterUnallowedNodeOptions([NodeTypeCode.VI], options)
        }

        return options;
    }

    public RecursivelyReconfigureNodeTypeOptions(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        this.ConfigureNodeTypeOptions(currentNode, nodeTypeOptions);

        const recurse = (childNodeReferences: INodeReferences) => {
            if (childNodeReferences.Length === 0) return;
            childNodeReferences.forEach((node: IPalavyrNode, index: number) => {
                this.ConfigureNodeTypeOptions(currentNode, nodeTypeOptions);
                recurse(node.childNodeReferences);
            });
        };

        recurse(currentNode.childNodeReferences);
    }

    public filterUnallowedNodeOptions(forbiddenOptions: Array<NodeTypeCode>, nodeTypeOptions: NodeTypeOptions) {
        const filteredNodeTypeOptions = nodeTypeOptions.filter((option: NodeOption) => {
            return !forbiddenOptions.includes(option.nodeTypeCode);
        });
        return filteredNodeTypeOptions;
    }
}

export default new NodeTypeOptionConfigurer();
