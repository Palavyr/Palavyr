import { ConvoNode, NodeTypeCode, NodeTypeOptions } from "@Palavyr-Types";
import { v4 as uuid } from "uuid";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { IPalavyrNode, IPalavyrLinkedList, INodeReferences } from "../../Contracts";
import { DEFAULT_NODE_TEXT } from "@constants";

export class NodeCreator {
    public addDefaultChild(currentParentNodes: IPalavyrNode[], optionPath: string, nodeTypeOptions: NodeTypeOptions, defaultText?: string) {
        if (currentParentNodes.length === 0) throw new Error("Attempting to add default child node to no parent nodes");

        const defaultNode = this.createDefaultNode(optionPath);
        const newPalavyrNode = currentParentNodes[0].palavyrLinkedList.convertToPalavyrNode(
            currentParentNodes[0].repository,
            defaultNode,
            currentParentNodes[0].setTreeWithHistory,
            currentParentNodes[0].isMemberOfLeftmostBranch
        );
        newPalavyrNode.setNodeTypeOptions(nodeTypeOptions);

        if (defaultText) {
            newPalavyrNode.userText = defaultText;
        }

        currentParentNodes.forEach((parentNode) => {
            parentNode.addNewNodeReferenceAndConfigure(newPalavyrNode, parentNode, nodeTypeOptions);
        });
    }

    public addDefaultRootNode(palavyrLinkedList: IPalavyrLinkedList, repository: PalavyrRepository, restOfTree: INodeReferences, defaultText?: string) {
        const defaultNode = this.createDefaultNode("Continue");
        defaultNode.isRoot = true;
        const newPalavyrNode = palavyrLinkedList.convertToPalavyrNode(repository, defaultNode, palavyrLinkedList.setTreeWithHistory, true);
        if (defaultText) {
            newPalavyrNode.userText = defaultText;
        }
        palavyrLinkedList.rootNode = newPalavyrNode;
        newPalavyrNode.childNodeReferences = restOfTree;
    }

    public createNewDefaultChildNode() {
        return this.createDefaultNode("Continue", "ProvideInfo");
    }

    private createDefaultNode(optionPath: string, nodeType: string = ""): ConvoNode {
        return {
            isLoopbackAnchorType: false,
            nodeId: uuid(),
            nodeType: nodeType,
            text: DEFAULT_NODE_TEXT,
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
