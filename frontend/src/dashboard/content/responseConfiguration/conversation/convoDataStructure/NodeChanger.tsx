import { NodeOption, NodeTypeCode } from "@Palavyr-Types";
import { IPalavyrNode } from "./Contracts";
import { NodeCreator } from "./NodeCreator";

export interface IPalavyrNodeChanger {
    ExecuteNodeSelectorUpdate(nodeOption: NodeOption, currentNode: IPalavyrNode): void;
    createOrTruncateChildNodes(currentNode: IPalavyrNode, valueOptions: string[]): void;
}

export class PalavyrNodeChanger implements IPalavyrNodeChanger {
    private nodeCreator: NodeCreator = new NodeCreator();
    constructor() {}

    public ExecuteNodeSelectorUpdate(nodeOption: NodeOption, currentNode: IPalavyrNode) {
        this.resetNodeProperties(nodeOption, currentNode);

        switch (nodeOption.nodeTypeCode) {
            case NodeTypeCode.I:
                this.ConvertToType_I_Node(nodeOption, currentNode);
                break;

            case NodeTypeCode.II:
                this.ConvertToType_II_Node(nodeOption, currentNode);
                break;

            case NodeTypeCode.III:
                this.ConvertToType_III_Node(nodeOption, currentNode);
                break;

            case NodeTypeCode.IV:
                this.ConvertToType_IV_Node(nodeOption, currentNode);
                break;

            case NodeTypeCode.V:
                this.ConvertToType_V_Node(nodeOption, currentNode);
                break;

            case NodeTypeCode.VI: // anabranch
                this.ConvertToType_VI_Node(nodeOption, currentNode);
                break;

            case NodeTypeCode.VII: // splitmerge
                break;

            default:
                throw new Error("NodeTypeCode unable to be identified. FIX THAT YO.");
        }
    }

    // Converts current node to a Type I node
    // Single Node - Option Frozen ([Terminal]) - No Children - Terminal

    // (Preconfigured Single node terminal (e.g. send repsonse) -- Type I
    // - No Child
    // - Single Node
    // - Options Frozen
    // - Terminal

    // Actions:
    // - remove child references (no child ref actions)
    // - set value options = ["Terminal"]

    private ConvertToType_I_Node(nodeOption: NodeOption, currentNode: IPalavyrNode) {
        currentNode.childNodeReferences.Clear();
        currentNode.setValueOptions(["Terminal"]);
        currentNode.UpdateTree();

    }

    // Type II
    // Single Node - Option Frozen ([CONTINUE]) - Single Child - Continue

    // (Preconfiged Single node continue, e.g. ProvideInfo, Image) -- Type II
    // - Single Child
    // - Single Node
    // - Options Frozen
    // - Continue

    // Actions:
    // -- truncate children to single leftmost child
    // -- set value Option to ["Continue"]
    // -- set child ref option to "Continue"

    private ConvertToType_II_Node(nodeOption: NodeOption, currentNode: IPalavyrNode) {
        const typeIIvalueOptions = ["Continue"];
        this.createOrTruncateChildNodes(currentNode, typeIIvalueOptions);
        currentNode.childNodeReferences.applyOptionPaths(typeIIvalueOptions);
        currentNode.setValueOptions(typeIIvalueOptions);
        currentNode.UpdateTree();

    }

    // Type III
    // Multi Node - Options Editable - Single Child - Continue

    // (MultiChoice Continue)  -- Type III
    // - Single Child
    // - Multi Node
    // - Options Editable
    // - Continue

    // Actions:
    // - set value options to child ref option paths
    // then
    // - truncate children to single leftmost child
    // Then
    // - set value option to ["Continue"]
    // - set check ref option path to 'Continue'

    // On Update:
    // - add value options
    // - do not add node references

    private ConvertToType_III_Node(nodeOption: NodeOption, currentNode: IPalavyrNode) {
        if (currentNode.childNodeReferences.Length === 0) {
            this.nodeCreator.addDefaultChild(currentNode, "Continue");
        } else {
            currentNode.childNodeReferences.truncateAt(1); // only take the first child node
            currentNode.childNodeReferences.applyOptionPaths(["Continue"]);
        }
        currentNode.UpdateTree();

    }

    // Type IV
    // Multi Node - Options Editable - Multiple Children - Continue

    //     (MultiChoice as Paths)  -- Type IV
    //     - Multi Children
    //     - Multi Node
    //     - Options Editable
    //     - Continue

    //    Actions:
    //     - set value options to current child reference options

    //    On Update:
    //     - add value options
    //     - add equal number of node references

    private ConvertToType_IV_Node(nodeOption: NodeOption, currentNode: IPalavyrNode) {
        // available choices - take from

        // child reference
        this.createOrTruncateChildNodes(currentNode, currentNode.getValueOptions());
        currentNode.UpdateTree();

    }

    //     Type V
    //     Multi Node - Options Frozen - Multiple Children - Continue

    //     (Preconfiged multi, e.g. YesNo, YesNoNotSure) -- Type V
    //     - MultiChildren
    //     - Multi Node
    //     - Options Frozen
    //     - Continue

    //    Actions:
    //     - set value options to nodeOption  value options
    //     - set child references to node Option value options

    //    On Update:
    //     - remove current child node references

    private ConvertToType_V_Node(nodeOption: NodeOption, currentNode: IPalavyrNode) {
        // available choices
        currentNode.setValueOptions(nodeOption.valueOptions);

        // child references
        this.createOrTruncateChildNodes(currentNode, nodeOption.valueOptions);

        // child ref choice outcome labels
        currentNode.childNodeReferences.applyOptionPaths(nodeOption.valueOptions);
        currentNode.UpdateTree();

    }

    // Type VI
    // Multi Node - Options Editable - Multiple Children - Continue

    //  -Anabranch - Type IV -- very similary to Type IV
    //     - Multi Children
    //     - Multi Node
    //     - Options Editable
    //     - Continue

    //    Actions:
    //     - set value options to current child reference options

    //    On Update:
    //     - add value options
    //     - add equal number of node references
    private ConvertToType_VI_Node(nodeOption: NodeOption, currentNode: IPalavyrNode) {
        if (currentNode.getValueOptions().length < 2) {
            const defaultValueOptions = ["Left Branch", "Right Branch"];
            this.createOrTruncateChildNodes(currentNode, defaultValueOptions);
            currentNode.childNodeReferences.applyOptionPaths(defaultValueOptions);
        } else {
            this.createOrTruncateChildNodes(currentNode, currentNode.getValueOptions());
            currentNode.childNodeReferences.applyOptionPaths(currentNode.getValueOptions());
        }
        currentNode.palavyrLinkedList.reconfigureTree();
    }

    public async createOrTruncateChildNodes(currentNode: IPalavyrNode, valueOptions: string[]) {
        if (currentNode.getValueOptions().length === 0) {
            this.nodeCreator.addDefaultChild(currentNode, "Continue");
        } else {
            const valueOptionDifference = valueOptions.length === 0 ? 0 : valueOptions.length - currentNode.childNodeReferences.Length;
            if (valueOptionDifference > 0) {
                // add nodes
                for (let index = 0; index < valueOptionDifference; index++) {
                    this.nodeCreator.addDefaultChild(currentNode, "Continue"); // autoreferences the parent and child
                }
            } else if (valueOptionDifference < 0) {
                // truncate nodes
                currentNode.childNodeReferences.truncateAt(valueOptionDifference);
            }
        }
    }

    private resetNodeProperties(nodeOption: NodeOption, currentNode: IPalavyrNode) {
        currentNode.nodeTypeCode = nodeOption.nodeTypeCode;
        currentNode.nodeType = nodeOption.value;
        currentNode.isCurrency = nodeOption.isCurrency;
        currentNode.isDynamicTableNode = nodeOption.isDynamicType;
        currentNode.isMultiOptionType = nodeOption.isMultiOptionType;
        currentNode.isSplitMergeType = nodeOption.isSplitMergeType;
        currentNode.isTerminal = nodeOption.isTerminalType;
        currentNode.nodeComponentType = nodeOption.nodeComponentType;
        currentNode.resolveOrder = nodeOption.resolveOrder; // IS this right?
        currentNode.shouldRenderChildren = nodeOption.shouldRenderChildren;
        currentNode.shouldShowMultiOption = nodeOption.shouldShowMultiOption;
        currentNode.dynamicType = nodeOption.dynamicType;
        currentNode.isAnabranchType = nodeOption.isAnabranchType;
        currentNode.isImageNode = nodeOption.isImageNode;
    }
}