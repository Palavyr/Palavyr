import { NodeOption, NodeTypeCode, NodeTypeOptions } from "@Palavyr-Types";
import { INodeReferences, IPalavyrNode } from "./Contracts";

// filterUnallowedNodeOptions(forbiddenOptions: Array<NodeTypeCode>, nodeTypeOptions: NodeTypeOptions);

class NodeTypeOptionConfigurer {
    constructor() {}

    public ConfigureNodeTypeOptions(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        if (currentNode.isPalavyrAnabranchMember) {
            return this.filterUnallowedNodeOptions([NodeTypeCode.VI, NodeTypeCode.VII], nodeTypeOptions);
        }
        return nodeTypeOptions;
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
