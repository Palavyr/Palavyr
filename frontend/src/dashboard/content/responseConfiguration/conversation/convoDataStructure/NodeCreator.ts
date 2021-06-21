import { IPalavyrNode } from "./Contracts";
import { ConvoNode, NodeTypeCode } from "@Palavyr-Types";
import { v4 as uuid } from "uuid";

export class NodeCreator {

    public addDefaultChild(currentNode: IPalavyrNode, optionPath: string) {
        const defaultNode = this.createDefaultNode(optionPath);
        const newPalavyrNode = currentNode.palavyrLinkedList.convertToPalavyrNode(
            currentNode.palavyrLinkedList,
            currentNode.repository,
            currentNode.nodeTypeOptions,
            defaultNode,
            currentNode.rawNodeList,
            currentNode.setTreeWithHistory,
            currentNode.isMemberOfLeftmostBranch
        );
        currentNode.addNewNodeReferenceAndConfigure(newPalavyrNode, currentNode);
    }

    createDefaultNode(optionPath: string): ConvoNode {
        return {
            IsSplitMergeMergePoint: false,
            nodeId: uuid(),
            nodeType: "",
            text: "Ask your question!",
            nodeChildrenString: "",
            isRoot: false,
            fallback: false,
            isCritical: false,
            areaIdentifier: "",
            optionPath: optionPath,
            valueOptions: "",
            isMultiOptionType: false,
            isTerminalType: false,
            isSplitMergeType: false,
            shouldRenderChildren: true,
            shouldShowMultiOption: false,
            isAnabranchMergePoint: false,
            isAnabranchType: false,
            nodeComponentType: "",
            isDynamicTableNode: false,
            resolveOrder: 0,
            dynamicType: "",
            isImageNode: false,
            imageId: null,
            nodeTypeCode: NodeTypeCode.I,
        };
    }
}
