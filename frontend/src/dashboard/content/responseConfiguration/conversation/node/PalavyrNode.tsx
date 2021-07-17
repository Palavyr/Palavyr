import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { ConvoNode, NodeTypeOptions, ValueOptionDelimiter, LineMap, AnabranchContext, LineLink, NodeTypeCode, LoopbackContext } from "@Palavyr-Types";
import { INodeReferences, IPalavyrLinkedList, IPalavyrNode } from "../Contracts";
import { NodeReferences } from "./PalavyrNodeReferences";
import { NodeConfigurer } from "./actions/NodeConfigurer";
import { NodeCreator } from "./actions/NodeCreator";

export class PalavyrNode implements IPalavyrNode {
    // used in widget resource
    public isRoot: boolean;
    public nodeId: string;
    public isTerminal: boolean;
    public shouldPresentResponse: boolean; // isCritical
    public nodeType: string; // type of node - e.g. YesNo, Outer-Categories-TwoNestedCategory-fffeefb5-36f2-40cd-96c1-f1eff401393c
    public isMultiOptionType: boolean;
    public isDynamicTableNode: boolean;
    public userText: string; // text
    public resolveOrder: number;
    public nodeComponentType: string; // type of component to use in the widget - standardized list of types in the widget registry
    public dynamicType: string | null; // generic dynamic type, e.g. SelectOneFlat-3242-2342-234-2423
    public nodeTypeOptions: NodeTypeOptions;
    public valueOptions: string[]; // the options available from this node, if any. I none, then "Continue" is used |peg| delimted
    public optionPath: string; // the value option that was used with the parent of this node.

    // transient
    public shouldRenderChildren: boolean;
    public shouldShowMultiOption: boolean;
    public isImageNode: boolean;
    public imageId: string | null;
    public nodeTypeCode: NodeTypeCode;
    public isCurrency: boolean;

    // core
    public childNodeReferences: INodeReferences = new NodeReferences();
    public parentNodeReferences: INodeReferences = new NodeReferences();
    private configurer = new NodeConfigurer();
    private nodeCreator: NodeCreator = new NodeCreator();

    public isMemberOfLeftmostBranch: boolean;
    public lineMap: LineMap = [];

    public setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void;
    public repository: PalavyrRepository;
    public palavyrLinkedList: IPalavyrLinkedList; // the containing list object that this node is a member of. Used to acccess update methods

    public shouldDisableNodeTypeSelector: boolean;

    // NEW PROPERTIES TO DEAL WITH anabranch and loopback

    // ANA BRANCH (we get isAnabranch from the DB. We should infer the rest here when constructing the linked list)
    public isAnabranchType: boolean;
    public isAnabranchMergePoint: boolean;

    public isPalavyrAnabranchStart: boolean;
    public isPalavyrAnabranchMember: boolean;
    public isPalavyrAnabranchEnd: boolean;
    public isAnabranchLocked: boolean;
    public anabranchContext: AnabranchContext;

    // LOOPBACK
    public isLoopbackAnchorType: boolean;

public isLoopbackMember: boolean; // only non-leftmostbranch
    public isLoopbackStart: boolean;
    public loopbackContext: LoopbackContext;

    constructor(containerList: IPalavyrLinkedList, repository: PalavyrRepository, node: ConvoNode, setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void, leftmostBranch: boolean) {
        this.repository = repository;
        this.palavyrLinkedList = containerList;

        this.optionPath = node.optionPath; // the value option that was used with the parent of this node.
        this.valueOptions = node.valueOptions.split(ValueOptionDelimiter); // the options available from this node, if any. I none, then "Continue" is used |peg| delimted

        // convo node
        this.isRoot = node.isRoot;
        this.nodeId = node.nodeId;
        this.isTerminal = node.isTerminalType;
        this.isMultiOptionType = node.isMultiOptionType;
        this.userText = node.text; // text
        this.shouldPresentResponse = node.isCritical; // isCritical
        this.isDynamicTableNode = node.isDynamicTableNode;
        this.resolveOrder = node.resolveOrder;
        this.shouldRenderChildren = node.shouldRenderChildren;
        this.nodeType = node.nodeType; // type of node - e.g. YesNo, Outer-Categories-TwoNestedCategory-fffeefb5-36f2-40cd-96c1-f1eff401393c
        this.nodeTypeCode = node.nodeTypeCode;
        this.nodeComponentType = node.nodeComponentType; // type of component to use in the widget - standardized list of types in the widget registry
        this.dynamicType = node.dynamicType; // generic dynamic type, e.g. SelectOneFlat-3242-2342-234-2423
        this.isImageNode = node.isImageNode;
        this.imageId = node.imageId;
        this.shouldShowMultiOption = node.shouldShowMultiOption;
        this.isAnabranchType = node.isAnabranchType;
        this.isAnabranchMergePoint = node.isAnabranchMergePoint;
        this.isLoopbackAnchorType = node.isLoopbackAnchorType;
        this.isMemberOfLeftmostBranch = leftmostBranch;
        this.setTreeWithHistory = setTreeWithHistory;
        this.isAnabranchLocked = false;
        this.shouldDisableNodeTypeSelector = false;
    }

    public UpdateTree() {
        if (this) {
            this.setTreeWithHistory(this.palavyrLinkedList);
        }
    }

    public unsetSelf(nodeTypeOptions: NodeTypeOptions) {
        if (this.isRoot) {
            this.childNodeReferences.Clear();
            this.palavyrLinkedList.resetRootNode();
        } else {
            const currentText = this.userText;
            this.childNodeReferences.Clear();
            this.parentNodeReferences.forEach((parentNode) => {
                parentNode.childNodeReferences.removeReference(this);
                this.nodeCreator.addDefaultChild([parentNode], this.optionPath, nodeTypeOptions, currentText);
                parentNode.sortChildReferences();
            });
        }
        this.UpdateTree();
    }

    public nodeIsSet() {
        return !isNullOrUndefinedOrWhitespace(this.nodeType);
    }

    public nodeIsNotSet() {
        return isNullOrUndefinedOrWhitespace(this.nodeType);
    }

    public setValueOptions(newValueOptions: string[]) {
        this.valueOptions = newValueOptions;
    }

    public addValueOption(newOption: string) {
        this.valueOptions.push(newOption);
    }

    public getValueOptions() {
        return this.valueOptions;
    }

    public sortChildReferences() {
        if (!this.isPalavyrAnabranchStart && !this.isLoopbackAnchorType && this.nodeType !== "MultipleChoiceAsPath") {
            this.childNodeReferences.OrderByOptionPath();
        }
    }

    public addNewNodeReferenceAndConfigure(newNode: IPalavyrNode, parentNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions) {
        // double linked
        parentNode.childNodeReferences.addReference(newNode);
        this.configurer.configure(newNode, parentNode, nodeTypeOptions);
    }

    public setNodeTypeOptions(newNodeTypeOptions: NodeTypeOptions): void {
        this.nodeTypeOptions = newNodeTypeOptions;
    }

    public AddNewChildReference(newChildReference: IPalavyrNode) {
        this.childNodeReferences.addReference(newChildReference);
    }

    public updateNodeText(newText: string) {
        this.userText = newText;
    }

    public getParentNodes() {
        return this.parentNodeReferences;
    }

    public getChildNodes() {
        return this.childNodeReferences;
    }

    public containsChildReference(node: IPalavyrNode) {
        return this.childNodeReferences.contains(node.nodeId);
    }

    public addLine(parentId: string) {
        const currentToValues = this.lineMap.map((x: LineLink) => x.to);
        if (!currentToValues.includes(parentId)) {
            const newLineLink: LineLink = {
                from: this.nodeId,
                to: parentId,
            };
            this.lineMap.push(newLineLink);
        }
    }

    public compileConvoNode(areaId: string): ConvoNode {
        // returns an object resource that matches the database schema
        return {
            areaIdentifier: areaId,
            isRoot: this.isRoot,
            nodeId: this.nodeId,
            text: this.userText,
            nodeType: this.nodeType,
            nodeComponentType: this.nodeComponentType,
            nodeChildrenString: this.childNodeReferences.joinedReferenceString,
            isCritical: this.shouldPresentResponse,
            optionPath: this.optionPath,
            valueOptions: this.valueOptions.join(ValueOptionDelimiter),
            isDynamicTableNode: this.isDynamicTableNode,
            dynamicType: this.dynamicType,
            resolveOrder: this.resolveOrder,
            isTerminalType: this.isTerminal,
            shouldRenderChildren: this.shouldRenderChildren,
            isMultiOptionType: this.isMultiOptionType,
            shouldShowMultiOption: this.shouldShowMultiOption,
            isAnabranchType: this.isAnabranchType,
            isAnabranchMergePoint: this.isAnabranchMergePoint,
            isImageNode: this.isImageNode,
            imageId: this.imageId,
            nodeTypeCode: this.nodeTypeCode,
            isLoopbackAnchorType: this.isLoopbackAnchorType,
        };
    }

    public LoopbackContextIsSet() {
        if (this.loopbackContext) {
            return this.loopbackContext.loopbackOriginId !== "";
        } else return false;
    }

    public lock() {
        this.isAnabranchLocked = true;
        this.shouldDisableNodeTypeSelector = true;
    }
    public unlock() {
        // TODO: either make this anabranch specific or don't do; anabranch lock here. LOck means uneditable? Or just no nodeType changes?
        this.isAnabranchLocked = false;
        this.shouldDisableNodeTypeSelector = false;
    }

    public setAsProvideInfo() {
        this.nodeType = "ProvideInfo";
    }

    public Equals(otherNode: IPalavyrNode) {
        return this.nodeId === otherNode.nodeId;
    }

    public recursiveReferenceThisAnabranchOrigin(anabranchMergeNode: IPalavyrNode) {
        if (!this.isAnabranchType) throw new Error("Attempting to call anabranch reference method from non-anabranch-origin node");
        this.lock();
        anabranchMergeNode.lock();

        const recurseAndReference = (childReferences: INodeReferences) => {
            childReferences.forEach((node: IPalavyrNode) => {
                node.lock();
                if (node.Equals(anabranchMergeNode)) {
                    // do nothing?
                    return;
                } else if (node.childNodeReferences.containsNode(anabranchMergeNode)) {
                    // reconfigure the siblings of the anabranchMergeNode
                    recurseAndReference(node.childNodeReferences.Where((node: IPalavyrNode) => !node.Equals(anabranchMergeNode)));
                } else {
                    if (node.childNodeReferences.Length === 1 && node.childNodeReferences.Single().nodeIsNotSet()) {
                        node.childNodeReferences.Clear();

                        if (!node.isTerminal) {
                            node.AddNewChildReference(anabranchMergeNode);
                        }

                        node.shouldRenderChildren = false;
                        anabranchMergeNode.addLine(node.nodeId);
                        anabranchMergeNode.parentNodeReferences.addReference(node);
                    } else if (node.nodeIsNotSet()) {
                        node.setAsProvideInfo();
                        node.shouldRenderChildren = false;
                        node.AddNewChildReference(anabranchMergeNode);
                        anabranchMergeNode.addLine(node.nodeId);
                        anabranchMergeNode.parentNodeReferences.addReference(node);
                    } else {
                        recurseAndReference(node.childNodeReferences);
                    }
                }
            });
        };

        // this is te anabranch origin node
        recurseAndReference(this.childNodeReferences);
    }

    public removeLine(toNode: IPalavyrNode) {
        this.lineMap = this.lineMap.filter((lineLink: LineLink) => {
            return lineLink.to !== toNode.nodeId;
        });
    }

    public dereferenceThisAnabranchMergePoint(anabranchOriginNode: IPalavyrNode, nodeTypeOptions: NodeTypeOptions): void {
        // Assumes that this node is the anabranch merge node with the checkbox
        if (!this.isAnabranchMergePoint) throw new Error("Attempting to call anabranch reference method from non-anabranch-merge-point node");

        anabranchOriginNode.unlock();
        this.unlock();
        const mergeNode = this as IPalavyrNode;

        const recurseAndDereference = (childReferences: INodeReferences) => {
            childReferences.forEach((node: IPalavyrNode) => {
                node.unlock();
                if (node.Equals(mergeNode)) {
                    recurseAndDereference(node.childNodeReferences);
                } else if (node.childNodeReferences.containsNode(mergeNode)) {
                    if (!node.anabranchContext.leftmostAnabranch) {
                        this.parentNodeReferences.removeReference(node);
                        node.childNodeReferences.Clear();
                        this.nodeCreator.addDefaultChild([node], "Continue", nodeTypeOptions);
                        node.shouldRenderChildren = true;
                    } else {
                        // ignore the mergeNode in the same way as the other
                        recurseAndDereference(node.childNodeReferences.Where((nodeRef: IPalavyrNode) => !nodeRef.Equals(mergeNode)));
                    }
                } else {
                    if (node.nodeType === "Loopback") {
                        return;
                    }
                    recurseAndDereference(node.childNodeReferences);
                }
            });
        };

        this.parentNodeReferences.forEach((node: IPalavyrNode, index: number) => {
            if (index > 0) this.removeLine(node);
        });

        recurseAndDereference(anabranchOriginNode.childNodeReferences);
    }
}
