import { ConversationDesignerNodeResource, NodeTypeCodeEnum, NodeTypeOptionResources } from "@Palavyr-Types";
import { v4 as uuid } from 'uuid';
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { IPalavyrNode, IPalavyrLinkedList, INodeReferences } from "@Palavyr-Types";
import { DEFAULT_NODE_TEXT } from "@constants";

export class NodeCreator {
    public addDefaultChild(currentParentNodes: IPalavyrNode[], optionPath: string, nodeTypeOptions: NodeTypeOptionResources, defaultText?: string) {
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
        const newPalavyrNode = palavyrLinkedList.convertToPalavyrNode(repository, defaultNode, palavyrLinkedList.updateTree, true);
        if (defaultText) {
            newPalavyrNode.userText = defaultText;
        }
        palavyrLinkedList.rootNode = newPalavyrNode;
        newPalavyrNode.childNodeReferences = restOfTree;
    }

    public createNewDefaultChildNode() {
        return this.createDefaultNode("Continue", "ProvideInfo");
    }

    private createDefaultNode(optionPath: string, nodeType: string = ""): ConversationDesignerNodeResource {
        return {
            isLoopbackAnchorType: false,
            nodeId: uuid(),
            nodeType: nodeType,
            text: DEFAULT_NODE_TEXT(),
            nodeChildrenString: "",
            isRoot: false,
            isCritical: false,
            intentId: "",
            optionPath: optionPath,
            valueOptions: "",
            isMultiOptionType: false,
            isTerminalType: false,
            shouldRenderChildren: true,
            shouldShowMultiOption: false,
            isAnabranchMergePoint: false,
            isAnabranchType: false,
            nodeComponentType: "ProvideInfo",
            isPricingStrategyNode: false,
            resolveOrder: 0,
            pricingStrategyType: "",
            isImageNode: false,
            fileId: "",
            nodeTypeCodeEnum: NodeTypeCodeEnum.I,
            isMultiOptionEditable: false,
            isCurrency: false,
            id: null
        };
    }
}
