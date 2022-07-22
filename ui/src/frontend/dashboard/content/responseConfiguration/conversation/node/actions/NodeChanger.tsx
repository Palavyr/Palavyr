import { NodeTypeOptionResource, NodeTypeOptionResources } from "@common/types/api/ApiContracts";
import { NodeTypeCodeEnum } from "@common/types/api/Enums";
import { INodeReferences, IPalavyrNode } from "@Palavyr-Types";
import AnabranchConfigurer from "./AnabranchConfigurer";
import { NodeCreator } from "./NodeCreator";

export interface IPalavyrNodeChanger {
    ExecuteNodeSelectorUpdate(nodeOption: NodeTypeOptionResource, currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources): void;
    createOrTruncateChildNodes(currentNode: IPalavyrNode, valueOptions: string[], nodeTypeOptions: NodeTypeOptionResources): void;
}

class PalavyrNodeChanger implements IPalavyrNodeChanger {
    private nodeCreator: NodeCreator = new NodeCreator();
    constructor() {}

    public ExecuteNodeSelectorUpdate(nodeOption: NodeTypeOptionResource, currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        if (currentNode.nodeType === "Loopback" && currentNode.nodeTypeCodeEnum !== nodeOption.nodeTypeCodeEnum) {
            currentNode.childNodeReferences.Clear();
            this.nodeCreator.addDefaultChild([currentNode], "Continue", nodeTypeOptions);
        }

        if (currentNode.nodeTypeCodeEnum === NodeTypeCodeEnum.VI && nodeOption.nodeTypeCodeEnum !== NodeTypeCodeEnum.VI) {
            AnabranchConfigurer.ClearAnabranchContext(currentNode);
        }

        if (currentNode.nodeTypeCodeEnum === NodeTypeCodeEnum.VII && nodeOption.nodeTypeCodeEnum !== NodeTypeCodeEnum.VII) {
            const recurse = (childNodeReferences: INodeReferences) => {
                childNodeReferences.forEach((childNode: IPalavyrNode) => {
                    if (childNode.nodeType === "Loopback") {
                        childNode.unsetSelf(nodeTypeOptions);
                    }
                    recurse(childNode.childNodeReferences);
                });
            };
            recurse(currentNode.childNodeReferences);
        }

        if (currentNode.nodeTypeCodeEnum === NodeTypeCodeEnum.IX && nodeOption.nodeTypeCodeEnum !== NodeTypeCodeEnum.IX) {
            currentNode = this.ConvertToTextNode(currentNode, nodeTypeOptions);
            currentNode.fileId = undefined;
        }

        this.resetNodeProperties(nodeOption, currentNode);

        switch (nodeOption.nodeTypeCodeEnum) {
            case NodeTypeCodeEnum.I:
                this.ConvertToType_I_Node(currentNode, nodeTypeOptions);
                break;

            case NodeTypeCodeEnum.II:
                this.ConvertToType_II_Node(currentNode, nodeTypeOptions);
                break;

            case NodeTypeCodeEnum.III:
                this.ConvertToType_III_Node(currentNode, nodeTypeOptions);
                break;

            case NodeTypeCodeEnum.IV:
                this.ConvertToType_IV_Node(currentNode, nodeTypeOptions);
                break;

            case NodeTypeCodeEnum.V:
                this.ConvertToType_V_Node(nodeOption, currentNode, nodeTypeOptions);
                break;

            case NodeTypeCodeEnum.VI: // anabranch
                this.ConvertToType_VI_Node(currentNode, nodeTypeOptions);
                break;

            case NodeTypeCodeEnum.VII: // loopback anchor
                this.ConvertToType_VII_Node(currentNode, nodeTypeOptions);
                break;

            case NodeTypeCodeEnum.VIII: // loopback point
                this.ConvertToType_VIII_Node(currentNode, nodeTypeOptions);
                break;

            case NodeTypeCodeEnum.IX: // image node
                this.ConvertToType_IX_Node(currentNode, nodeTypeOptions);
                break;

            case NodeTypeCodeEnum.X:
                this.ConvertToType_X_Node(nodeOption, currentNode, nodeTypeOptions);
                break;

            // Type XI - where you have value options being supplied by the nodeOption (a pricing strategy node Option)
            case NodeTypeCodeEnum.XI:
                this.ConvertToType_XI_Node(nodeOption, currentNode, nodeTypeOptions);
                break;

            default:
                throw new Error("nodeTypeCodeEnum unable to be identified. FIX THAT YO.");
        }
        if (currentNode.parentNodeReferences.Length === 1) {
            const parentNode = currentNode.parentNodeReferences.retrieveLeftmostReference()!;
            parentNode.sortChildReferences();
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

    private ConvertToType_I_Node(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        currentNode.childNodeReferences.Clear();
        currentNode.setValueOptions(["Terminal"]);
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
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

    private ConvertToType_II_Node(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        const typeIIvalueOptions = ["Continue"];
        this.createOrTruncateChildNodes(currentNode, typeIIvalueOptions, nodeTypeOptions);
        currentNode.childNodeReferences.applyOptionPaths(typeIIvalueOptions);
        currentNode.setValueOptions(typeIIvalueOptions);
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
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

    private ConvertToType_III_Node(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        if (currentNode.childNodeReferences.Length === 0) {
            this.nodeCreator.addDefaultChild([currentNode], "Continue", nodeTypeOptions);
        } else {
            currentNode.childNodeReferences.truncateAt(1); // only take the first child node
            currentNode.childNodeReferences.applyOptionPaths(["Continue"]);
        }
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
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

    private ConvertToType_IV_Node(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        // child reference
        this.createOrTruncateChildNodes(currentNode, currentNode.getValueOptions(), nodeTypeOptions);
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
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

    private ConvertToType_V_Node(nodeOption: NodeTypeOptionResource, currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        // available choices
        currentNode.setValueOptions(nodeOption.valueOptions);

        // child references
        this.createOrTruncateChildNodes(currentNode, nodeOption.valueOptions, nodeTypeOptions);

        // child ref choice outcome labels
        currentNode.childNodeReferences.applyOptionPaths(nodeOption.valueOptions);
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
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
    private ConvertToType_VI_Node(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        if (currentNode.getValueOptions().length < 2) {
            const defaultValueOptions = ["Left Branch", "Right Branch"];
            this.createOrTruncateChildNodes(currentNode, defaultValueOptions, nodeTypeOptions);
            currentNode.childNodeReferences.applyOptionPaths(defaultValueOptions);
        } else {
            this.createOrTruncateChildNodes(currentNode, currentNode.getValueOptions(), nodeTypeOptions);
            currentNode.childNodeReferences.applyOptionPaths(currentNode.getValueOptions());
        }

        const recurse = (childNodeReference: IPalavyrNode) => {
            if (childNodeReference.isTerminal) {
                childNodeReference.unsetSelf(nodeTypeOptions);
                return;
            }
            if (childNodeReference.nodeType === "Loopback") return;
            if (childNodeReference.childNodeReferences.Length > 0) {
                recurse(childNodeReference.childNodeReferences.retrieveLeftmostReference()!);
            }
        };

        if (currentNode.childNodeReferences.Length > 0) {
            recurse(currentNode.childNodeReferences.retrieveLeftmostReference()!);
        }

        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
    }

    // Type VII
    // Loopback Anchor
    private ConvertToType_VII_Node(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        if (currentNode.getValueOptions().length < 2) {
            const defaultValueOptions = ["Continue", "Option 1"];
            this.createOrTruncateChildNodes(currentNode, defaultValueOptions, nodeTypeOptions);
            currentNode.childNodeReferences.applyOptionPaths(defaultValueOptions);
        } else {
            this.createOrTruncateChildNodes(currentNode, currentNode.getValueOptions(), nodeTypeOptions);
            currentNode.childNodeReferences.applyOptionPaths(currentNode.getValueOptions());
        }
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions); // todo check this is correct
    }

    // Type VIII
    // The loopback terminal
    private ConvertToType_VIII_Node(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        // else its the loopback endpoint
        const parentRef = currentNode.parentNodeReferences.retrieveLeftmostReference();
        if (parentRef !== null && parentRef.isLoopbackMember) {
            let originId: string;
            if (parentRef.isLoopbackStart) {
                originId = parentRef.nodeId;
            } else {
                originId = parentRef.loopbackContext.loopbackOriginId;
            }
            const originNode = currentNode.palavyrLinkedList.findNode(originId!)!;
            if (originNode == null) throw new Error("Could not find loopback origin node.");
            currentNode.childNodeReferences.Clear();
            currentNode.childNodeReferences.addReference(originNode);
        }
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
    }

    // Type IX
    // The Image Type node
    // No longer useing different derived node type.
    // This is Node type == IX and imageId is not null (its set)
    private ConvertToType_IX_Node(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        this.ConvertToImageNode(currentNode, nodeTypeOptions);
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
    }

    // Type X
    //
    private ConvertToType_X_Node(nodeOption: NodeTypeOptionResource, currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        this.createOrTruncateChildNodes(currentNode, ["Continue"], nodeTypeOptions);
        currentNode.setValueOptions(nodeOption.valueOptions);
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
    }

    private ConvertToType_XI_Node(nodeOption: NodeTypeOptionResource, currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        this.createOrTruncateChildNodes(currentNode, nodeOption.valueOptions, nodeTypeOptions);
        currentNode.setValueOptions(nodeOption.valueOptions);
        currentNode.childNodeReferences.applyOptionPaths(nodeOption.valueOptions);
        currentNode.palavyrLinkedList.reconfigureTree(nodeTypeOptions);
    }

    private ConvertToImageNode(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        const newImageNode = currentNode.palavyrLinkedList.convertToPalavyrNode(
            currentNode.repository,
            currentNode.compileConvoNode(currentNode.palavyrLinkedList.intentId),
            currentNode.setTreeWithHistory,
            currentNode.isMemberOfLeftmostBranch
        );

        newImageNode.parentNodeReferences = currentNode.parentNodeReferences;
        newImageNode.childNodeReferences = currentNode.childNodeReferences;
        currentNode.parentNodeReferences.forEach((parentNode: IPalavyrNode) => {
            parentNode.childNodeReferences.removeReference(currentNode);
            parentNode.childNodeReferences.addReference(newImageNode);
            parentNode.sortChildReferences();
            newImageNode.addLine(parentNode.nodeId);
        });
        currentNode.childNodeReferences.forEach((childNode: IPalavyrNode) => {
            childNode.parentNodeReferences.removeReference(currentNode);
            childNode.parentNodeReferences.addReference(newImageNode);
        });

        this.createOrTruncateChildNodes(newImageNode, ["Continue"], nodeTypeOptions);
        if (newImageNode.isRoot) {
            newImageNode.palavyrLinkedList.rootNode = newImageNode;
        }
        newImageNode.fileId = null;
        newImageNode.childNodeReferences.applyOptionPaths(["Continue"]);
    }

    private ConvertToTextNode(currentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptionResources) {
        const newTextNode = currentNode.palavyrLinkedList.convertToPalavyrNode(
            currentNode.repository,
            currentNode.compileConvoNode(currentNode.palavyrLinkedList.intentId),
            currentNode.setTreeWithHistory,
            currentNode.isMemberOfLeftmostBranch
        );
        newTextNode.parentNodeReferences = currentNode.parentNodeReferences;
        newTextNode.childNodeReferences = currentNode.childNodeReferences;
        currentNode.parentNodeReferences.forEach((parentNode: IPalavyrNode) => {
            parentNode.childNodeReferences.removeReference(currentNode);
            parentNode.childNodeReferences.addReference(newTextNode);
            newTextNode.addLine(parentNode.nodeId);
            parentNode.sortChildReferences();
        });
        currentNode.childNodeReferences.forEach((childNode: IPalavyrNode) => {
            childNode.parentNodeReferences.removeReference(currentNode);
            childNode.parentNodeReferences.addReference(newTextNode);
        });
        this.createOrTruncateChildNodes(newTextNode, ["Continue"], nodeTypeOptions);

        if (newTextNode.isRoot) {
            newTextNode.palavyrLinkedList.rootNode = newTextNode;
        }

        return newTextNode;
    }

    public async createOrTruncateChildNodes(currentNode: IPalavyrNode, valueOptions: string[], nodeTypeOptions: NodeTypeOptionResources) {
        if (currentNode.getValueOptions().length === 0) {
            this.nodeCreator.addDefaultChild([currentNode], "Continue", nodeTypeOptions);
        } else {
            const valueOptionDifference = valueOptions.length === 0 ? 0 : valueOptions.length - currentNode.childNodeReferences.Length;
            if (valueOptionDifference > 0) {
                // add nodes
                for (let index = 0; index < valueOptionDifference; index++) {
                    this.nodeCreator.addDefaultChild([currentNode], "Continue", nodeTypeOptions); // autoreferences the parent and child
                }
            } else if (valueOptionDifference < 0) {
                // truncate nodes
                currentNode.childNodeReferences.truncateAt(valueOptionDifference);
            }
        }
    }

    private resetNodeProperties(nodeOption: NodeTypeOptionResource, currentNode: IPalavyrNode) {
        currentNode.nodeTypeCodeEnum = nodeOption.nodeTypeCodeEnum;
        currentNode.nodeType = nodeOption.value;
        currentNode.isCurrency = nodeOption.isCurrency;
        currentNode.isPricingStrategyNode = nodeOption.isPricingStrategyType;
        currentNode.isMultiOptionType = nodeOption.isMultiOptionType;
        currentNode.isTerminal = nodeOption.isTerminalType;
        currentNode.nodeComponentType = nodeOption.nodeComponentType;
        currentNode.resolveOrder = nodeOption.resolveOrder; // IS this right?
        currentNode.shouldRenderChildren = nodeOption.shouldRenderChildren;
        currentNode.shouldShowMultiOption = nodeOption.shouldShowMultiOption;
        currentNode.isAnabranchType = nodeOption.isAnabranchType;
        currentNode.isImageNode = nodeOption.isImageNode;
        currentNode.pricingStrategyType = nodeOption.pricingStrategyType;
        currentNode.isLoopbackAnchorType = nodeOption.isLoopbackAnchor;
    }
}

export default new PalavyrNodeChanger();
