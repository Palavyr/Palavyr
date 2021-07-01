import { IPalavyrNode } from "./Contracts";
import { ConvoNode, NodeTypeCode, NodeTypeOptions } from "@Palavyr-Types";
import { v4 as uuid } from "uuid";

export class NodeCreator {
    public addDefaultChild(currentNode: IPalavyrNode, optionPath: string, nodeTypeOptions: NodeTypeOptions) {
        const defaultNode = this.createDefaultNode(optionPath);
        const newPalavyrNode = currentNode.palavyrLinkedList.convertToPalavyrNode(
            currentNode.palavyrLinkedList,
            currentNode.repository,
            defaultNode,
            currentNode.rawNodeList,
            currentNode.setTreeWithHistory,
            currentNode.isMemberOfLeftmostBranch
        );
        newPalavyrNode.setNodeTypeOptions(nodeTypeOptions);
        currentNode.addNewNodeReferenceAndConfigure(newPalavyrNode, currentNode, nodeTypeOptions);
    }

    createDefaultNode(optionPath: string): ConvoNode {
        return {
            isLoopbackAnchorType: false,
            nodeId: uuid(),
            nodeType: "",
            text: "Ask your question!",
            nodeChildrenString: "",
            isRoot: false,
            isCritical: false,
            areaIdentifier: "",
            optionPath: optionPath,
            valueOptions: "",
            isMultiOptionType: false,
            isTerminalType: false,
            shouldRenderChildren: true,
            shouldShowMultiOption: false,
            isAnabranchMergePoint: false,
            isAnabranchType: false,
            nodeComponentType: "ProvideInfo",
            isDynamicTableNode: false,
            resolveOrder: 0,
            dynamicType: "",
            isImageNode: false,
            imageId: null,
            nodeTypeCode: NodeTypeCode.I,
        };
    }
}
