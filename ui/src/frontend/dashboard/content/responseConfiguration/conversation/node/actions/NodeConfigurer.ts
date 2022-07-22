import { NodeTypeCodeEnum, NodeTypeOptionResources } from "@Palavyr-Types";
import { IPalavyrNode } from "@Palavyr-Types";
import AnabranchConfigurer from "./AnabranchConfigurer";
import LoopbackAnchorConfigurer from "./LoopbackAnchorConfigurer";

export class NodeConfigurer {
    constructor() {}

    public configure(currentNode: IPalavyrNode, parentNode: IPalavyrNode | null = null, nodeTypeOptions: NodeTypeOptionResources) {
        if (currentNode.isRoot) {
            AnabranchConfigurer.configureAnabranchWhenRoot(currentNode);
            LoopbackAnchorConfigurer.ConfigureLoopbackAnchorWhenRoot(currentNode);
        } else if (parentNode !== null) {
            currentNode.parentNodeReferences.addReference(parentNode);

            if (currentNode.nodeTypeCodeEnum !== NodeTypeCodeEnum.VII) {
                currentNode.addLine(parentNode.nodeId);
            }
            if (currentNode.nodeTypeCodeEnum === NodeTypeCodeEnum.VII) {
                if (parentNode.nodeTypeCodeEnum !== NodeTypeCodeEnum.VIII) {
                    currentNode.addLine(parentNode.nodeId);
                }
            }
            AnabranchConfigurer.configureAnabranch(currentNode, parentNode, nodeTypeOptions);
            LoopbackAnchorConfigurer.ConfigureLoopbackAnchor(currentNode, parentNode);
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
}
