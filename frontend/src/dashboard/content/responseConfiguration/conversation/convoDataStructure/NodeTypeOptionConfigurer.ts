import { NodeTypeCode, NodeTypeOptions } from "@Palavyr-Types";
import { IPalavyrNode } from "./Contracts";

class NodeTypeOptionConfigurer {
    constructor() {}

    public ConfigureNodeTypeOptions(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        switch (currentNode.nodeTypeCode) {
            case NodeTypeCode.VI:
                return currentNode.filterUnallowedNodeOptions([NodeTypeCode.VI, NodeTypeCode.VII], nodeTypeOptions);
            default:
                return nodeTypeOptions;
        }
    }
}

export default new NodeTypeOptionConfigurer();
