import React, { useState, useEffect } from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { Card, CardContent, makeStyles, Typography } from "@material-ui/core";
import { ConvoNode, NodeTypeOptions, ValueOptionDelimiter, AlertType, NodeOption } from "@Palavyr-Types";
import classNames from "classnames";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { DataLogging } from "../nodes/nodeInterface/nodeDebug/DataLogging";
import { CustomNodeSelect } from "../nodes/nodeInterface/nodeSelector/CustomNodeSelect";
import { SteppedLineTo } from "../treeLines/SteppedLineTo";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { PalavyrImageNode } from "./PalavyrImageNode";
import { PalavyrLinkedList } from "./PalavyrLinkedList";
import { PalavyrNodeOptionals } from "./PalavyrNodeOptionals";
import { PalavyrTextNode } from "./PalavyrTextNode";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";

type ComponentType = React.ComponentType<{}>;

interface IPalavyrNode {
    createPalavyrNodeComponent(): ComponentType;
}

export type LineLink = {
    from: string;
    to: string;
};
export type LineMap = LineLink[];

export type SplitmergeContext = {
    splitmergeOriginId: string; // the node Id of the split merge root node
};

export type AnabranchContext = {
    anabranchOriginId: string; // the node Id of the anabranch root node
};

export abstract class PalavyrNode implements IPalavyrNode {
    // used in widget resource
    public isRoot: boolean;
    public nodeId: string;

    public isTerminal: boolean;
    public shouldPresentResponse: boolean; // isCritical
    public nodeType: string; // type of node - e.g. YesNo, Outer-Categories-TwoNestedCategory-fffeefb5-36f2-40cd-96c1-f1eff401393c
    public isMultiOptionType: boolean;

    protected userText: string; // text
    protected isDynamicTableNode: boolean;
    protected resolveOrder: number;
    protected nodeComponentType: string; // type of component to use in the widget - standardized list of types in the widget registry
    protected dynamicType: string | null; // generic dynamic type, e.g. SelectOneFlat-3242-2342-234-2423
    public nodeChildrenString: string;

    protected valueOptions: string[]; // the options available from this node, if any. I none, then "Continue" is used |peg| delimted
    public optionPath: string; // the value option that was used with the parent of this node.

    // transient
    public shouldRenderChildren: boolean;
    protected isSplitMergeType: boolean;
    protected shouldShowMultiOption: boolean;
    protected isAnabranchType: boolean;
    protected isAnabranchMergePoint: boolean;
    protected isImageNode: boolean;
    protected imageId: string | null;

    public isCurrency: boolean;

    // core
    public childNodeReferences: NodeReferences = new NodeReferences();
    public parentNodeReferences: NodeReferences = new NodeReferences();

    public isMemberOfLeftmostBranch: boolean;

    public rawNode: ConvoNode; // Get Rid Of This.
    public rawNodeList: ConvoNode[]; // Get Rid of this

    public lineMap: LineMap = [];

    public setTreeWithHistory: (updatedTree: PalavyrLinkedList) => void;
    protected repository: PalavyrRepository;
    public palavyrLinkedList: PalavyrLinkedList; // the containing list object that this node is a member of. Used to acccess update methods

    // deprecated
    protected fallback: boolean;
    protected nodeTypeOptions: NodeTypeOptions;
    private shouldDisableNodeTypeSelector: boolean;

    // NEW PROPERTIES TO DEAL WITH anabranch and split merge types --

    // ANA BRANCH (we get isAnabranch from the DB. We should infer the rest here when constructing the linked list)
    public isPalavyrAnabranchStart: boolean;
    public isPalavyrAnabranchMember: boolean;
    public isPalavyrAnabranchEnd: boolean;
    public isAnabranchLocked: boolean;
    public anabranchContext: AnabranchContext;

    // SPLIT MERGE (we also get isSplitMerge from Db, so samesy - infer the rest here)
    public isPalavyrSplitmergeStart: boolean;
    public isPalavyrSplitmergeMember: boolean;
    public isPalavyrSplitmergePrimarybranch: boolean;
    public isPalavyrSplitmergeEnd: boolean;
    public isPalavyrSplitmergeMergePoint: boolean;
    public splitmergeContext: SplitmergeContext;

    /**
     * this node type will hold a reference to the parent nodes
     * We could have multiple parents (e.g. anabranch)
     * We could have no parents if root

     * We hold a reference to the child nodes
     * We could have multiple children
     * We could have no children (if not set)
     **/

    constructor(
        containerList: PalavyrLinkedList,
        nodeTypeOptions: NodeTypeOptions,
        repository: PalavyrRepository,
        node: ConvoNode,
        nodeList: ConvoNode[],
        setTreeWithHistory: (updatedTree: PalavyrLinkedList) => void,
        leftmostBranch: boolean
    ) {
        this.repository = repository;
        this.palavyrLinkedList = containerList;

        this.nodeTypeOptions = nodeTypeOptions;

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
        this.nodeComponentType = node.nodeComponentType; // type of component to use in the widget - standardized list of types in the widget registry
        this.dynamicType = node.dynamicType; // generic dynamic type, e.g. SelectOneFlat-3242-2342-234-2423

        this.nodeChildrenString = this.childNodeReferences.joinedReferenceString;
        this.shouldShowMultiOption = node.shouldShowMultiOption;

        this.isSplitMergeType = node.isSplitMergeType;
        this.isPalavyrSplitmergeMergePoint = node.IsSplitMergeMergePoint;

        this.isAnabranchType = node.isAnabranchType;
        this.isAnabranchMergePoint = node.isAnabranchMergePoint;
        this.isImageNode = node.isImageNode;
        this.imageId = node.imageId;

        this.isMemberOfLeftmostBranch = leftmostBranch;

        this.rawNode = node;
        this.rawNodeList = nodeList;
        this.setTreeWithHistory = setTreeWithHistory;

        this.isAnabranchLocked = false; // TODO set this dynamically
        this.shouldDisableNodeTypeSelector = false; // todo set this dynamically

        // deprecated
        this.fallback = node.fallback;
    }

    public UpdateTree() {
        this.setTreeWithHistory(this.palavyrLinkedList);
    }

    public removeSelf() {
        this.parentNodeReferences.removeReference(this);
    }

    public nodeIsSet() {
        return !isNullOrUndefinedOrWhitespace(this.nodeType);
    }

    public nodeIsNotSet() {
        return isNullOrUndefinedOrWhitespace(this.nodeType);
    }

    public configure(parentNode: PalavyrNode) {
        this.parentNodeReferences.addReference(parentNode);
        this.addLine(parentNode.nodeId);
        this.configureAnabranch(parentNode);
        this.configureSplitMerge(parentNode);
    }

    private configureAnabranch(parentNode: PalavyrNode) {
        this.isPalavyrAnabranchStart = this.isAnabranchType;
        this.isPalavyrAnabranchMember = parentNode.isAnabranchType || (parentNode.isPalavyrAnabranchMember && !parentNode.isPalavyrAnabranchEnd) || this.isAnabranchType || this.isAnabranchMergePoint;
        this.isPalavyrAnabranchEnd = this.isAnabranchMergePoint;

        if (this.isPalavyrAnabranchStart) {
            this.anabranchContext = {
                ...parentNode.anabranchContext,
                anabranchOriginId: this.nodeId,
            };
        }
    }

    private configureSplitMerge(parentNode: PalavyrNode) {
        this.isPalavyrSplitmergeStart = this.isSplitMergeType;
        this.isPalavyrSplitmergeMember = parentNode.isSplitMergeType || (parentNode.isPalavyrSplitmergeMember && !parentNode.isPalavyrSplitmergeEnd);
        this.isPalavyrSplitmergePrimarybranch = this.isMemberOfLeftmostBranch;
        this.isPalavyrSplitmergeEnd = this.isPalavyrSplitmergeMergePoint;

        if (this.isPalavyrAnabranchStart) {
            this.splitmergeContext = {
                ...parentNode.splitmergeContext,
                splitmergeOriginId: this.nodeId,
            };
        }
    }

    public sortChildReferences() {
        // reorder parent's child refs depending on if parent is a anabranch or splitmerge type
        if (!this.isAnabranchType && !this.isSplitMergeType) {
            this.childNodeReferences.OrderByOptionPath();
        }
    }

    public async addDefaultChild(optionPath: string) {
        // const defaultNode = (await this.repository.Conversations.GetDefaultConversationNode()) as ConvoNode;
        const defaultNode: ConvoNode = {
            IsSplitMergeMergePoint: false,
            nodeId: "",
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
        };
        const newPalavyrNode = PalavyrNode.convertToPalavyrNode(
            this.palavyrLinkedList,
            this.repository,
            this.nodeTypeOptions,
            defaultNode,
            this.rawNodeList,
            this.setTreeWithHistory,
            this.isMemberOfLeftmostBranch
        );
        this.addNewNodeReferenceAndConfigure(newPalavyrNode, this);
        this.UpdateTree();
    }

    public addNewNodeReferenceAndConfigure(newNode: PalavyrNode, parentNode: PalavyrNode) {
        // double linked
        parentNode.childNodeReferences.addReference(newNode);
        newNode.configure(parentNode);
    }

    public AddNewChildReference(newChildReference: PalavyrNode) {
        this.childNodeReferences.addReference(newChildReference);
    }

    public static convertToPalavyrNode(
        container: PalavyrLinkedList,
        repository: PalavyrRepository,
        nodeTypeOptions: NodeTypeOptions,
        rawNode: ConvoNode,
        nodeList: ConvoNode[],
        setTreeWithHistory: (updatedTree: PalavyrLinkedList) => void,
        leftMostBranch: boolean
    ) {
        let palavyrNode: PalavyrNode;
        switch (rawNode.isImageNode) {
            case true:
                palavyrNode = new PalavyrImageNode(container, nodeTypeOptions, repository, rawNode, nodeList, setTreeWithHistory, leftMostBranch);
                break;
            case false:
                palavyrNode = new PalavyrTextNode(container, nodeTypeOptions, repository, rawNode, nodeList, setTreeWithHistory, leftMostBranch);
                break;
            default:
                throw new Error("Node type couldn't be determined when construting the palavyr convo tree.");
        }
        return palavyrNode;
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

    public containsChildReference(node: PalavyrNode) {
        return this.childNodeReferences.contains(node.nodeId);
    }

    public addLine(parentId: string) {
        const newLineLink: LineLink = {
            from: this.nodeId,
            to: parentId,
        };
        this.lineMap.push(newLineLink);
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
            fallback: this.fallback,
            isSplitMergeType: this.isSplitMergeType,
            IsSplitMergeMergePoint: this.isPalavyrSplitmergeMergePoint,
            shouldShowMultiOption: this.shouldShowMultiOption,
            isAnabranchType: this.isAnabranchType,
            isAnabranchMergePoint: this.isAnabranchMergePoint,
            isImageNode: this.isImageNode,
            imageId: this.imageId,
        };
    }

    private compileDebug(): { [key: string]: string }[] {
        // this will return an array of objects that will be used to preset debug data
        const { ...object } = this;
        return Object.keys(object).map((key: string) => {
            return {
                [key]: object[key],
            };
        });
    }

    // TODO: Would this work: https://sourceforge.net/projects/js-graph-it/ ?
    public createPalavyrNodeComponent() {
        return () => {
            return (
                <>
                    <div className={`tree-item tree-item-${this.nodeId}`}>
                        <div className="tree-block-wrap">{this.renderNodeInterface()()}</div>
                        <div className="tree-row">
                            {this.childNodeReferences.NotEmpty() &&
                                (this.shouldRenderChildren ? (
                                    this.childNodeReferences.nodes.map(
                                        (nextNode: PalavyrNode): React.ReactNode => {
                                            const Node = nextNode.createPalavyrNodeComponent();
                                            return <Node key={nextNode.nodeId} />;
                                        }
                                    )
                                ) : (
                                    <></>
                                ))}
                        </div>
                    </div>
                    {this.lineMap.map((line: LineLink) => {
                        return <SteppedLineTo key={`${line.to}-${line.from}-stepped-line`} from={line.from} to={line.to} />;
                    })}
                </>
            );
        };
    }

    private renderPalavyrNodeTypeSelector() {
        return () => {
            const [alertState, setAlertState] = useState<boolean>(false);
            const [alertDetails, setAlertDetails] = useState<AlertType>();
            const [label, setLabel] = useState<string>("");

            useEffect(() => {
                const currentNodeOption = this.nodeTypeOptions.filter((option: NodeOption) => option.value === this.nodeType)[0];
                if (currentNodeOption) {
                    setLabel(currentNodeOption.text);
                }
            }, [this.nodeType]);

            const duplicateDynamicFeeNodeFound = (option: string) => {
                const dynamicNodeTypeOptions = this.nodeTypeOptions.filter((x: NodeOption) => x.isDynamicType);
                if (dynamicNodeTypeOptions.length > 0) {
                    const dynamicNodeTypes = dynamicNodeTypeOptions.map((x: NodeOption) => x.value);
                    const nodeList = this.palavyrLinkedList.compileToConvoNodes(); // Write methods to handle this natively - this is a bit of a cheat atm.
                    const dynamicNodesPresentInTheCurrentNodeList = nodeList.filter((x: ConvoNode) => dynamicNodeTypes.includes(x.nodeType));
                    const dynamicNodes = dynamicNodesPresentInTheCurrentNodeList.map((x: ConvoNode) => x.nodeType);
                    return dynamicNodes.includes(option);
                }
                return false;
            };

            const autocompleteOnChange = async (_: any, nodeOption: NodeOption) => {
                if (nodeOption === null) {
                    return;
                }

                if (duplicateDynamicFeeNodeFound(nodeOption.value)) {
                    setAlertDetails({
                        title: `You've already placed dynamic table ${nodeOption.text} in this conversation`,
                        message:
                            "You can only place each dynamic table in your conversation once. If you would like to change where you've placed it in the conversation, you need to recreate that portion of the tree by selection a different node.",
                    });
                    setAlertState(true);
                    return;
                }

                // changeNodeType(node, nodeList, setNodes, nodeOption, nodeIdentity, selectionCallback);
                this.convertTo(nodeOption);
                this.setTreeWithHistory(this.palavyrLinkedList);
            };

            return (
                <>
                    <CustomNodeSelect onChange={autocompleteOnChange} label={label} nodeTypeOptions={this.nodeTypeOptions} shouldDisabledNodeTypeSelector={this.shouldDisableNodeTypeSelector} />
                    {alertDetails && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />}
                </>
            );
        };
    }

    private convertTo(nodeOption: NodeOption) {
        this.nodeType = nodeOption.value;
        this.isCurrency = nodeOption.isCurrency;
        this.isDynamicTableNode = nodeOption.isDynamicType;
        this.isMultiOptionType = nodeOption.isMultiOptionType;
        this.isSplitMergeType = nodeOption.isSplitMergeType;
        this.isTerminal = nodeOption.isTerminalType;
        this.nodeComponentType = nodeOption.nodeComponentType;
        this.resolveOrder = nodeOption.resolveOrder; // IS this right?
        this.shouldRenderChildren = nodeOption.shouldRenderChildren;
        this.shouldShowMultiOption = nodeOption.shouldShowMultiOption;
        this.valueOptions = nodeOption.valueOptions;
        this.dynamicType = nodeOption.dynamicType;
    }

    abstract renderNodeEditor(): ({ editorIsOpen, closeEditor }) => JSX.Element;
    abstract renderNodeFace(): ({ openEditor }) => JSX.Element;
    private renderNodeInterface() {
        return () => {
            const { showDebugData } = React.useContext(ConversationTreeContext);

            const [editorIsOpen, setEditorState] = useState<boolean>(false);
            const openEditor = () => setEditorState(true);
            const closeEditor = () => setEditorState(false);

            const cls = useNodeInterfaceStyles({
                nodeType: this.nodeType,
                nodeText: this.userText,
                checked: this.shouldPresentResponse,
                isDecendentOfSplitMerge: this.isPalavyrSplitmergeMember,
                splitMergeRootSiblingIndex: this.isMemberOfLeftmostBranch ? 0 : 1,
                debugOn: showDebugData,
                isImageNode: this.imageId !== null,
            });

            return (
                <Card className={classNames(cls.root, this.nodeId)} variant="outlined">
                    <CardContent className={cls.card}>
                        {showDebugData && <DataLogging debugData={this.compileDebug()} nodeChildren={this.nodeChildrenString} nodeId={this.nodeId} />}
                        {this.renderNodeHeader()({ isRoot: this.isRoot, optionPath: this.optionPath })}
                        {this.renderNodeFace()({ openEditor })}
                        {this.renderPalavyrNodeTypeSelector()()}
                        {this.renderNodeEditor()({ editorIsOpen, closeEditor })}
                        {this.renderOptionals()()}
                    </CardContent>
                </Card>
            );
        };
    }

    private renderNodeHeader() {
        const useStyles = makeStyles((theme) => ({
            interfaceElement: {
                paddingBottom: "1rem",
            },
        }));
        interface INodeInterfaceHeader {
            isRoot: boolean;
            optionPath: string;
        }
        return ({ isRoot, optionPath }: INodeInterfaceHeader) => {
            const cls = useStyles();
            return (
                <Typography className={cls.interfaceElement} variant={isRoot ? "h5" : "body1"} align="center">
                    {isRoot ? "Begin" : optionPath === "Continue" ? optionPath : "If " + optionPath}
                </Typography>
            );
        };
    }

    public RouteToMostRecentSplitMerge() {
        this.childNodeReferences.Clear();
        let parentReferences = this.parentNodeReferences;

        const findMostRecentSplitMergeAndAssign = (parentNode: PalavyrNode) => {
            let found = false;
            while (true) {
                for (let index = 0; index < parentReferences.Length; index++) {
                    if (parentNode.isSplitMergeType) {
                        this.childNodeReferences.addReference(parentNode);
                        found = true;
                        return true;
                    } else if (parentNode.isRoot) {
                        throw new Error("Failed to find the Split Merge origin - logic error.");
                    } else {
                        findMostRecentSplitMergeAndAssign(parentNode);
                    }
                    parentNode = parentReferences.getByIndex(index);
                }
            }
        };

        for (let index = 0; index < parentReferences.Length; index++) {
            const parent = parentReferences.getByIndex(index);
            const result = findMostRecentSplitMergeAndAssign(parent);
            if (result) {
                console.log("UPDATED THE TREE AFTER SPLIT MERGE UPDATE");
                this.shouldRenderChildren = false;
                this.UpdateTree();
                break; // early break
            }
        }
    }

    private lock() {
        this.isAnabranchLocked = true;
        this.shouldDisableNodeTypeSelector = true;
    }
    private unlock() {
        // TODO: either make this anabranch specific or don't do; anabranch lock here. LOck means uneditable? Or just no nodeType changes?
        this.isAnabranchLocked = false;
        this.shouldDisableNodeTypeSelector = false;
    }

    private setAsProvideInfo() {
        this.nodeType = "ProvideInfo";
    }

    public recursiveReferenceThisAnabranchOrigin(anabranchMergeNode: PalavyrNode) {
        if (!this.isAnabranchType) throw new Error("Attempting to call anabranch reference method from non-anabranch-origin node");

        const recurseAndReference = (childReferences: NodeReferences) => {
            for (let index = 0; index < childReferences.Length; index++) {
                const childNode = childReferences.references[index];
                childNode.lock();
                if (childNode.nodeIsNotSet()) {
                    childNode.AddNewChildReference(anabranchMergeNode);
                    anabranchMergeNode.parentNodeReferences.addReference(childNode);
                    childNode.setAsProvideInfo();
                } else {
                    recurseAndReference(childNode.childNodeReferences);
                }
            }
        };

        recurseAndReference(this.childNodeReferences);
    }

    public recursiveDereferenceThisAnabranchOrigin(anabranchMergeNode: PalavyrNode) {
        if (!this.isAnabranchType) throw new Error("Attempting to call anabranch reference method from non-anabranch-origin node");

        const recurseAndDereference = (childReferences: NodeReferences) => {
            for (let index = 0; index < childReferences.Length; index++) {
                const childNode = childReferences.references[index];
                childNode.unlock();

                const childReferencesAnabranchMerge = childNode.childNodeReferences.checkIfReferenceExistsOnCondition((x: PalavyrNode) => x.nodeId === anabranchMergeNode.nodeId);
                if (childReferencesAnabranchMerge && !childNode.isMemberOfLeftmostBranch) {
                    childNode.childNodeReferences.removeReference(anabranchMergeNode);
                    anabranchMergeNode.parentNodeReferences.removeReference(childNode);
                } else {
                    recurseAndDereference(childNode.childNodeReferences);
                }
            }
        };

        recurseAndDereference(this.childNodeReferences);
    }

    public isPenultimate() {
        if (this.childNodeReferences.Empty()) return false;

        for (let index = 0; index < this.childNodeReferences.Length; index++) {
            const childNodeReference = this.childNodeReferences.references[index];
            if (childNodeReference.childNodeReferences.Length !== 0) {
                return false; // check this logic TODO:!
            }
            return true;
        }
    }

    private renderOptionals() {
        const nodeOptionals = new PalavyrNodeOptionals(this);

        return () => {
            return (
                <>
                    {nodeOptionals.renderShowResponseInPdf()()}
                    {nodeOptionals.renderShowMergeWithPrimarySiblingBranchOption()()}
                    {nodeOptionals.renderAnabranchMergeCheckBox()()}
                    {nodeOptionals.renderUnsetNodeButton()()}
                    {nodeOptionals.renderSplitMergeAnchorLabel()()}
                    {nodeOptionals.renderAnabranchMergeNodeLabel()()}
                </>
            );
        };
    }
}

export class NodeReferences {
    private nodeReferences: PalavyrNode[] = [];

    constructor(nodeReferences?: PalavyrNode[]) {
        if (nodeReferences) {
            nodeReferences.forEach((ref: PalavyrNode) => {
                this.addReference(ref);
            });
        }
    }

    public get nodes() {
        return this.nodeReferences;
    }

    public get joinedReferenceString() {
        return this.joinNodeChildrenStringArray(this.referenceStringArray);
    }

    public get referenceStringArray() {
        return this.nodeReferences.map((x) => x.nodeId);
    }

    public get references() {
        return this.nodeReferences;
    }

    public get Length() {
        return this.nodeReferences.length;
    }

    private joinNodeChildrenStringArray(nodeChildrenStrings: string[]) {
        return nodeChildrenStrings.join(",");
    }

    public contains(nodeId: string) {
        return this.nodeReferences.filter((x: PalavyrNode) => x.nodeId === nodeId).length > 0;
    }

    public addReference(node: PalavyrNode) {
        if (!this.contains(node.nodeId)) {
            this.nodeReferences.push(node);
        }
    }

    public Empty() {
        return this.nodeReferences.length === 0;
    }

    public NotEmpty() {
        return this.nodeReferences.length > 0;
    }

    public OrderByOptionPath() {
        this.nodeReferences = sortByPropertyAlphabetical((x: PalavyrNode) => x.optionPath.toUpperCase(), this.nodeReferences);
    }

    public Clear() {
        this.nodeReferences = [];
    }

    public getByIndex(index: number) {
        try {
            return this.nodeReferences[index];
        } catch {
            throw new Error(`Failed to find node reference index: Index: ${index} out of range ${this.Length}`);
        }
    }

    public removeReference(palavyrNode: PalavyrNode) {
        this.nodeReferences.filter((node: PalavyrNode) => node.nodeId !== palavyrNode.nodeId);
    }

    public checkIfReferenceExistsOnCondition(condition: (nodeReference: PalavyrNode) => boolean) {
        const result = this.nodeReferences.map(condition);
        return result.some((x) => x); // TODO: Check this works;
    }
}
