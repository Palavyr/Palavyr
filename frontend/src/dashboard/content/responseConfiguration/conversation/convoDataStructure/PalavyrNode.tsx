import React, { useState, useEffect, useCallback, useContext } from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { Card, CardContent, Dialog, DialogActions, DialogContent, DialogTitle, Divider, makeStyles, TextField, Tooltip, Typography } from "@material-ui/core";
import { ConvoNode, NodeTypeOptions, ValueOptionDelimiter, AlertType, NodeOption, LineMap, AnabranchContext, SplitmergeContext, LineLink, FileLink, SetState } from "@Palavyr-Types";
import classNames from "classnames";
import { ConversationTreeContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import { DataLogging } from "../nodes/nodeInterface/nodeDebug/DataLogging";
import { CustomNodeSelect } from "../nodes/nodeInterface/nodeSelector/CustomNodeSelect";
import { SteppedLineTo } from "../treeLines/SteppedLineTo";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { INodeReferences, IPalavyrLinkedList, IPalavyrNode } from "./Contracts";
import { NodeReferences } from "./PalavyrNodeReferences";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { useHistory } from "react-router-dom";
import { Upload } from "../../uploadable/Upload";
import { CustomImage } from "../nodes/nodeInterface/nodeEditor/imageNode/CustomImage";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { MultiChoiceOptions } from "../nodes/nodeInterface/nodeEditor/MultiChoiceOptions";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { NodeCheckBox } from "../nodes/nodeInterface/NodeCheckBox";

import { createDefaultNode } from "./defaultNode";

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
    public childNodeReferences: INodeReferences = new NodeReferences();
    public parentNodeReferences: INodeReferences = new NodeReferences();

    public isMemberOfLeftmostBranch: boolean;

    public rawNode: ConvoNode; // Get Rid Of This.
    public rawNodeList: ConvoNode[]; // Get Rid of this

    public lineMap: LineMap = [];

    public setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void;
    protected repository: PalavyrRepository;
    public palavyrLinkedList: IPalavyrLinkedList; // the containing list object that this node is a member of. Used to acccess update methods

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
        containerList: IPalavyrLinkedList,
        nodeTypeOptions: NodeTypeOptions,
        repository: PalavyrRepository,
        node: ConvoNode,
        nodeList: ConvoNode[],
        setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void,
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

    public configure(parentNode: IPalavyrNode) {
        this.parentNodeReferences.addReference(parentNode);
        this.addLine(parentNode.nodeId);
        this.configureAnabranch(parentNode);
        this.configureSplitMerge(parentNode);
    }

    private configureAnabranch(parentNode: IPalavyrNode) {
        this.isPalavyrAnabranchStart = this.isPalavyrAnabranchStart;
        this.isPalavyrAnabranchMember =
            parentNode.isPalavyrAnabranchStart || (parentNode.isPalavyrAnabranchMember && !parentNode.isPalavyrAnabranchEnd) || this.isPalavyrAnabranchStart || this.isAnabranchMergePoint;
        this.isPalavyrAnabranchEnd = this.isAnabranchMergePoint;

        if (this.isPalavyrAnabranchStart) {
            this.anabranchContext = {
                ...parentNode.anabranchContext,
                anabranchOriginId: this.nodeId,
            };
        }
    }

    private configureSplitMerge(parentNode: IPalavyrNode) {
        this.isPalavyrSplitmergeStart = this.isPalavyrSplitmergeStart;
        this.isPalavyrSplitmergeMember = parentNode.isPalavyrSplitmergeStart || (parentNode.isPalavyrSplitmergeMember && !parentNode.isPalavyrSplitmergeEnd);
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
        if (!this.isPalavyrAnabranchStart && !this.isPalavyrSplitmergeStart) {
            this.childNodeReferences.OrderByOptionPath();
        }
    }

    public async addDefaultChild(optionPath: string) {
        const defaultNode = createDefaultNode(optionPath);
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
        // this.UpdateTree();
    }

    public addNewNodeReferenceAndConfigure(newNode: IPalavyrNode, parentNode: IPalavyrNode) {
        // double linked
        parentNode.childNodeReferences.addReference(newNode);
        newNode.configure(parentNode);
    }

    public AddNewChildReference(newChildReference: IPalavyrNode) {
        this.childNodeReferences.addReference(newChildReference);
    }

    public static convertToPalavyrNode(
        container: IPalavyrLinkedList,
        repository: PalavyrRepository,
        nodeTypeOptions: NodeTypeOptions,
        rawNode: ConvoNode,
        nodeList: ConvoNode[],
        setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void,
        leftMostBranch: boolean
    ) {
        let palavyrNode: IPalavyrNode;
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

    public containsChildReference(node: IPalavyrNode) {
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
        const useStyles = makeStyles((theme) => ({
            treeItem: {
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                zIndex: 10,
            },
            treeBlockWrap: {
                padding: "2rem 2rem 2rem 2rem",
            },
            treeRow: {
                display: "flex",
                flexDirection: "row",
            },
        }));
        return () => {
            const [loaded, setLoaded] = useState<boolean>(false);

            useEffect(() => {
                setLoaded(true);
                return () => setLoaded(false);
            }, []);

            const cls = useStyles();
            const treelinkClassName = "LineLink";
            return (
                <>
                    <div className={classNames(treelinkClassName, cls.treeItem)}>
                        <div className={cls.treeBlockWrap}>{this.renderNodeInterface()()}</div>
                        {this.childNodeReferences.NotEmpty() && (
                            <div key={this.nodeId} className={cls.treeRow}>
                                {this.shouldRenderChildren ? (
                                    this.childNodeReferences.nodes.map(
                                        (nextNode: IPalavyrNode, index: number): React.ReactNode => {
                                            const Node = nextNode.createPalavyrNodeComponent();
                                            return <Node key={nextNode.nodeId + index.toString()} />;
                                        }
                                    )
                                ) : (
                                    <></>
                                )}
                            </div>
                        )}
                    </div>
                    {loaded &&
                        this.lineMap.map((line: LineLink) => {
                            return <SteppedLineTo key={line.from} from={line.from} to={line.to} treeLinkClassName={treelinkClassName} />;
                        })}
                </>
            );
        };
    }

    protected renderPalavyrNodeBody() {
        type StyleProps = {
            nodeText: string;
            isImageNode: boolean;
        };

        const useStyles = makeStyles((theme) => ({
            interfaceElement: {
                paddingBottom: "1rem",
            },
            textCard: (props: StyleProps) => ({
                border: "1px solid gray",
                padding: "10px",
                textAlign: "center",
                color: props.nodeText === "Ask your question!" && !props.isImageNode ? "white" : "black",
                background: props.nodeText === "Ask your question!" && !props.isImageNode ? "red" : "white",
                "&:hover": {
                    background: "lightgray",
                    color: "black",
                },
            }),

            editorStyle: {
                fontSize: "12px",
                color: "lightgray",
            },
        }));

        interface INodeBody {
            children: React.ReactNode;
            openEditor(): void;
        }

        return ({ openEditor, children }: INodeBody) => {
            const cls = useStyles();
            return (
                <Card elevation={0} className={classNames(cls.interfaceElement, cls.textCard)} onClick={openEditor}>
                    {children}
                    <Typography align="center" className={cls.editorStyle} onClick={openEditor}>
                        Click to Edit
                    </Typography>
                </Card>
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

                this.createOrTruncateChildNodes(nodeOption);
                this.convertThisNodeTo(nodeOption);
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

    private createOrTruncateChildNodes(nodeOption: NodeOption) {
        //                                  3                                1         =  + 2     (we need to add 2 new default nodes)
        //                                  1                                 3         =  -2 (we need to truncate the last two nodes)

        const valueOptionDifference = nodeOption.valueOptions.length === 0 ? 0 : nodeOption.valueOptions.length - this.childNodeReferences.Length;

        if (valueOptionDifference > 0) {
            for (let index = 0; index < valueOptionDifference; index++) {
                this.addDefaultChild("default"); // autoreferences the parent and child
            }
        } else if (valueOptionDifference < 0) {
            this.childNodeReferences.truncateAt(valueOptionDifference);
        }
        this.updateChildNodePathOptions(nodeOption);
    }

    private updateChildNodePathOptions(nodeOption: NodeOption) {
        if (this.isMultiOptionType) {
            // yes/no, or mulitiopton continue/paths
            if (nodeOption.isMultiOptionType) {
                this.childNodeReferences.applyOptionPaths(nodeOption.valueOptions);
            } else {
                this.childNodeReferences.applyOptionPaths(["Continue"]);
            }
        } else {
            if (nodeOption.isMultiOptionType) {
                if (nodeOption.valueOptions.length > 0) {
                    this.childNodeReferences.applyOptionPaths(nodeOption.valueOptions);
                } else {
                    this.valueOptions = this.childNodeReferences.collectPathOptions();
                }
            } else {
                this.childNodeReferences.applyOptionPaths(["Continue"]);
            }
        }
    }

    private convertThisNodeTo(nodeOption: NodeOption) {
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

    private updateChildNodeReferences(valueOptions: string[]) {
        // need to transfer the value options to the path options;
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
                <Card style={{ border: "5px solid black" }} id={this.nodeId} className={cls.root} variant="outlined">
                    <CardContent className={classNames(cls.card, this.nodeId)}>
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

        const findMostRecentSplitMergeAndAssign = (parentNode: IPalavyrNode) => {
            let found = false;
            while (true) {
                for (let index = 0; index < parentReferences.Length; index++) {
                    if (parentNode.isPalavyrSplitmergeStart) {
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

    public recursiveReferenceThisAnabranchOrigin(anabranchMergeNode: IPalavyrNode) {
        if (!this.isAnabranchType) throw new Error("Attempting to call anabranch reference method from non-anabranch-origin node");

        const recurseAndReference = (childReferences: INodeReferences) => {
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

    public recursiveDereferenceThisAnabranchOrigin(anabranchMergeNode: IPalavyrNode) {
        if (!this.isAnabranchType) throw new Error("Attempting to call anabranch reference method from non-anabranch-origin node");

        const recurseAndDereference = (childReferences: INodeReferences) => {
            for (let index = 0; index < childReferences.Length; index++) {
                const childNode = childReferences.references[index];
                childNode.unlock();

                const childReferencesAnabranchMerge = childNode.childNodeReferences.checkIfReferenceExistsOnCondition((x: IPalavyrNode) => x.nodeId === anabranchMergeNode.nodeId);
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

export class PalavyrTextNode extends PalavyrNode {
    constructor(
        containerList: IPalavyrLinkedList,
        nodeTypeOptions: NodeTypeOptions,
        repository: PalavyrRepository,
        node: ConvoNode,
        nodeList: ConvoNode[],
        setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void,
        leftmostBranch: boolean
    ) {
        super(containerList, nodeTypeOptions, repository, node, nodeList, setTreeWithHistory, leftmostBranch);
    }

    public renderNodeFace() {
        const cls = useNodeInterfaceStyles();
        return ({ openEditor }) => {
            return this.renderPalavyrNodeBody()({
                openEditor,
                children: (
                    <Typography className={cls.text} variant="body2" component="span" noWrap={false}>
                        {this.userText}
                    </Typography>
                ),
            });
        };
    }

    public renderNodeEditor() {
        return ({ editorIsOpen, closeEditor }) => {
            const [options, setOptions] = useState<string[]>([]);
            const [textState, setText] = useState<string>("");

            const handleUpdateNode = (value: string, valueOptions: string[]) => {
                this.userText = value;
                if (this.isMultiOptionType) {
                    this.valueOptions = valueOptions;
                }
                this.setTreeWithHistory(this.palavyrLinkedList);
            };

            return (
                <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
                    <DialogTitle>Edit a conversation node</DialogTitle>
                    <DialogContent>{this.renderTextEditor(setText, setOptions, textState, options)()}</DialogContent>
                    <DialogActions>
                        <SaveOrCancel
                            position="right"
                            customSaveMessage="Node Text Updated"
                            customCancelMessage="Changes cancelled"
                            useSaveIcon={false}
                            saveText="Update Node Text"
                            onSave={async () => {
                                handleUpdateNode(textState, options);
                                closeEditor();
                                return true;
                            }}
                            onCancel={closeEditor}
                            timeout={200}
                        />
                    </DialogActions>
                </Dialog>
            );
        };
    }

    public renderTextEditor(setText: SetState<string>, setOptions: SetState<string[]>, textState: string, options: string[]) {
        return () => {
            const [switchState, setSwitchState] = useState<boolean>(true);

            useEffect(() => {
                setText(this.userText);
                if (this.isMultiOptionType) {
                    //&& !isNullOrUndefinedOrWhitespace(this.valueOptions)) {
                    setOptions(this.valueOptions);
                }
            }, [options]);

            return (
                <>
                    <TextField margin="dense" value={textState} multiline rows={4} onChange={(event) => setText(event.target.value)} id="question" label="Question or Information" type="text" fullWidth />
                    {this.renderMultiOptionInputs(setOptions, options, switchState, setSwitchState)()}
                </>
            );
        };
    }

    public renderMultiOptionInputs(setOptions: SetState<string[]>, options: string[], switchState: boolean, setSwitchState: SetState<boolean>) {
        return () => {
            const addMultiChoiceOptionsOnClick = () => {
                options.push("");
                setOptions(options);
                setSwitchState(!switchState);
            };
            return (
                <>
                    {this.isMultiOptionType && this.shouldShowMultiOption && (
                        <>
                            <MultiChoiceOptions options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} addMultiChoiceOptionsOnClick={addMultiChoiceOptionsOnClick} />
                        </>
                    )}
                </>
            );
        };
    }
}

export class PalavyrImageNode extends PalavyrNode {
    constructor(
        containerList: IPalavyrLinkedList,
        nodeTypeOptions: NodeTypeOptions,
        repository: PalavyrRepository,
        node: ConvoNode,
        nodeList: ConvoNode[],
        setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void,
        leftmostBranch: boolean
    ) {
        super(containerList, nodeTypeOptions, repository, node, nodeList, setTreeWithHistory, leftmostBranch);
    }

    public renderNodeFace() {
        return ({ openEditor }) => {
            const [imageLink, setImageLink] = useState<string>("");
            const [imageName, setImageName] = useState<string>("");
            const [currentImageId, setCurrentImageId] = useState<string>("");

            const loadImage = useCallback(async () => {
                if (this.imageId !== null && this.imageId !== undefined) {
                    const fileLinks = await this.repository.Configuration.Images.getImages([this.imageId]);
                    const fileLink = fileLinks[0];
                    if (!fileLink.isUrl) {
                        const presignedUrl = await this.repository.Configuration.Images.getSignedUrl(fileLink.link);
                        setImageLink(presignedUrl);
                        setImageName(fileLink.fileName);
                        setCurrentImageId(fileLink.fileId);
                    }
                }
            }, [this.palavyrLinkedList]);

            useEffect(() => {
                loadImage();
            }, [this.palavyrLinkedList]);

            return this.renderPalavyrNodeBody()({ openEditor, children: <CustomImage imageName={imageName} imageLink={imageLink} titleVariant="body1" /> });
        };
    }

    public renderNodeEditor() {
        return ({ editorIsOpen, closeEditor }) => {
            const [imageLink, setImageLink] = useState<string>("");
            const [imageName, setImageName] = useState<string>("");
            const [currentImageId, setCurrentImageId] = useState<string>("");

            const loadImage = useCallback(async () => {
                if (this.imageId !== null && this.imageId !== undefined) {
                    const fileLinks = await this.repository.Configuration.Images.getImages([this.imageId]);
                    const fileLink = fileLinks[0];
                    if (!fileLink.isUrl) {
                        const presignedUrl = await this.repository.Configuration.Images.getSignedUrl(fileLink.link);
                        setImageLink(presignedUrl);
                        setImageName(fileLink.fileName);
                        setCurrentImageId(fileLink.fileId);
                    }
                }
            }, [this.palavyrLinkedList]);

            useEffect(() => {
                loadImage();
            }, [this.palavyrLinkedList]);

            return (
                <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
                    <DialogTitle>Edit a conversation node</DialogTitle>
                    <DialogContent>
                        {this.imageId === null
                            ? this.renderImageEditorWhenEmpty(closeEditor, currentImageId, setImageLink, setImageName)()
                            : this.renderImageEditorWhenFull(closeEditor, imageName, imageLink, currentImageId, setImageLink, setImageName)()}
                    </DialogContent>
                </Dialog>
            );
        };
    }

    public renderImageEditorWhenEmpty(closeEditor, currentImageId, setImageLink, setImageName) {
        return () => {
            return (
                <>
                    <Typography align="center" variant="h6">
                        Upload an image
                    </Typography>
                    {this.renderImageUpload(closeEditor, currentImageId, setImageLink, setImageName, false)()}
                </>
            );
        };
    }

    public renderImageEditorWhenFull(closeEditor: () => void, imageName, imageLink, currentImageId, setImageLink, setImageName) {
        return () => {
            return (
                <>
                    <CustomImage imageName={imageName} imageLink={imageLink} />
                    <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                    <Typography align="center" variant="h6">
                        Choose a new image
                    </Typography>
                    {this.renderImageUpload(closeEditor, currentImageId, setImageLink, setImageName, false)()}
                    <Divider />
                </>
            );
        };
    }

    public renderImageUpload(closeEditor: () => void, currentImageId, setImageLink, setImageName, initialState = false) {
        return () => {
            const cls = useNodeInterfaceStyles();
            const history = useHistory();
            const [uploadModal, setUploadModal] = useState(false);

            const { setIsLoading, setSuccessOpen, setSuccessText, planTypeMeta } = useContext(DashboardContext);
            useEffect(() => {
                if (planTypeMeta && !planTypeMeta.allowedImageUpload) {
                    history.push("/dashboard/please-subscribe");
                }
            }, [planTypeMeta]);

            const toggleModal = () => {
                setUploadModal(!uploadModal);
            };

            const fileSave = async (files: File[]) => {
                setIsLoading(true);
                const formData = new FormData();

                let result: FileLink[];
                if ((files.length = 1)) {
                    formData.append("files", files[0]);
                    result = await this.repository.Configuration.Images.saveSingleImage(formData);
                    setSuccessText("Image Uploaded");
                } else if (files.length > 1) {
                    files.forEach((file: File) => {
                        formData.append("files", file);
                    });
                    result = await this.repository.Configuration.Images.saveMultipleImages(formData);
                    setSuccessText("Images Uploaded");
                } else {
                    return;
                }

                await this.repository.Configuration.Images.savePreExistingImage(result[0].fileId, this.nodeId);
                setIsLoading(false);
                setSuccessOpen(true);
                closeEditor();
            };

            return (
                <>
                    <div className={cls.imageBlock}>{this.renderSelectFromExistingImages(currentImageId, setImageLink, setImageName)()}</div>
                    <Divider />
                    <div className={cls.imageBlock}>
                        {planTypeMeta && planTypeMeta.allowedImageUpload && (
                            <Upload
                                dropzoneType="area"
                                initialState={initialState}
                                modalState={uploadModal}
                                toggleModal={() => toggleModal()}
                                handleFileSave={(files: File[]) => fileSave(files)}
                                summary="Upload a file."
                                buttonText="Upload"
                                uploadDetails={<Typography>Upload an image, pdf, or other document you wish to share with your users</Typography>}
                                acceptedFiles={["image/png", "image/jpg"]}
                            />
                        )}
                    </div>
                </>
            );
        };
    }

    public renderSelectFromExistingImages(currentImageId, setImageLink, setImageName) {
        return () => {
            const [options, setOptions] = useState<FileLink[] | null>(null);
            const [label, setLabel] = useState<string>("");

            const onChange = async (_: any, option: FileLink) => {
                const convoNode = await this.repository.Configuration.Images.savePreExistingImage(option.fileId, this.nodeId);
                setLabel(option.fileName);

                this.imageId = option.fileId;

                if (!option.isUrl) {
                    const presignedUrl = await this.repository.Configuration.Images.getSignedUrl(option.link);
                    setImageLink(presignedUrl);
                    setImageName(option.fileName);
                }
                this.setTreeWithHistory(this.palavyrLinkedList);
            };

            const groupGetter = (val: FileLink) => val.fileName;

            const loadOptions = useCallback(async () => {
                const fileLinks = await this.repository.Configuration.Images.getImages();
                const sortedOptions = sortByPropertyAlphabetical(groupGetter, fileLinks);
                const filteredOptions = sortedOptions.filter((link: FileLink) => {
                    return link.fileId !== currentImageId;
                });
                setOptions(filteredOptions);
            }, [currentImageId]);

            useEffect(() => {
                loadOptions();
            }, [currentImageId]);

            return (
                <PalavyrAccordian title="Select a file you've already uploaded" initialState={false}>
                    {options && <PalavyrAutoComplete label={label} options={options} shouldDisableSelect={false} onChange={onChange} getOptionLabel={(option) => option.fileName} />}
                </PalavyrAccordian>
            );
        };
    }
}

export class PalavyrNodeOptionals {
    private palavyrNode: IPalavyrNode;
    constructor(node: IPalavyrNode) {
        this.palavyrNode = node;
    }

    public renderSplitMergeAnchorLabel() {
        const shouldShow = this.palavyrNode.isPalavyrSplitmergeMergePoint;
        return () => {
            return shouldShow ? <Typography>This is the primary sibling. Branches will merge to this node.</Typography> : <></>;
        };
    }

    public renderAnabranchMergeCheckBox() {
        const disabled = this.palavyrNode.isPalavyrAnabranchStart && this.palavyrNode.isPalavyrAnabranchEnd;

        const onChange = (event: { target: { checked: boolean } }, setAnabranchMergeChecked: SetState<boolean>) => {
            const checked = event.target.checked;
            const origin = this.palavyrNode.anabranchContext.anabranchOriginId;
            const anabranchOriginNode = this.palavyrNode.palavyrLinkedList.findNode(origin);

            if (checked) {
                this.palavyrNode.isPalavyrAnabranchEnd = true;
                anabranchOriginNode.recursiveReferenceThisAnabranchOrigin(this.palavyrNode);
                setAnabranchMergeChecked(true);
            } else {
                this.palavyrNode.isPalavyrAnabranchEnd = false;
                setAnabranchMergeChecked(false);
                anabranchOriginNode.recursiveDereferenceThisAnabranchOrigin(this.palavyrNode);
            }
            this.palavyrNode.UpdateTree();
        };

        const shouldShow = () => {
            const isChildOfAnabranchType = this.palavyrNode.parentNodeReferences.checkIfReferenceExistsOnCondition((node: IPalavyrNode) => node.isPalavyrAnabranchStart);
            return (
                this.palavyrNode.isPalavyrSplitmergeMember && !this.palavyrNode.isTerminal && !isChildOfAnabranchType && (this.palavyrNode.isMemberOfLeftmostBranch || this.palavyrNode.isPalavyrAnabranchStart)
            ); // && decendentLevelFromAnabranch < 4; TODO
        };

        return () => {
            const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);

            return shouldShow() ? (
                <Tooltip title="This option locks nodes internal to the Anbranch. You cannot change node types when this is set.">
                    <NodeCheckBox disabled={disabled} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={(event) => onChange(event, setAnabranchMergeChecked)} />
                </Tooltip>
            ) : (
                <></>
            );
        };
    }

    public renderUnsetNodeButton() {
        const shouldShow = () => {
            return (
                this.palavyrNode.nodeIsSet() &&
                (!this.palavyrNode.isPalavyrAnabranchMember || this.palavyrNode.isAnabranchLocked) &&
                !this.palavyrNode.isPalavyrSplitmergeMergePoint &&
                !this.palavyrNode.isAnabranchLocked
            );
        };

        const onClick = () => {
            this.palavyrNode.removeSelf();
            this.palavyrNode.UpdateTree();
        };

        return () => {
            return shouldShow() ? <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={onClick} /> : <></>;
        };
    }

    public renderAnabranchMergeNodeLabel() {
        return () => {
            return this.palavyrNode.isPalavyrAnabranchEnd ? <Typography style={{ fontWeight: "bolder" }}>This is the Anabranch Merge Node</Typography> : <></>;
        };
    }

    public renderShowResponseInPdf() {
        const shouldShow = () => {
            const nodeTypesThatDoNotProvideFeedback = ["ProvideInfo"];
            return !this.palavyrNode.isTerminal && !nodeTypesThatDoNotProvideFeedback.includes(this.palavyrNode.nodeType);
        };
        const onChange = (event: { target: { checked: boolean } }) => {
            const checked = event.target.checked;
            this.palavyrNode.shouldPresentResponse = checked;
            this.palavyrNode.setTreeWithHistory(this.palavyrNode.palavyrLinkedList);
        };

        return () => {
            return shouldShow() ? <NodeCheckBox label="Show response in PDF" checked={this.palavyrNode.shouldPresentResponse} onChange={onChange} /> : <></>;
        };
    }

    public renderShowMergeWithPrimarySiblingBranchOption() {
        const shouldShow = () => {
            return (
                this.palavyrNode.isPalavyrSplitmergeMember &&
                this.palavyrNode.isPalavyrSplitmergePrimarybranch &&
                this.palavyrNode.nodeIsSet() &&
                !this.palavyrNode.isTerminal &&
                !this.palavyrNode.isMultiOptionType &&
                this.palavyrNode.isPenultimate()
            );
        };

        const onClick = async (event: { target: { checked: boolean } }, setMergeBoxChecked: SetState<boolean>) => {
            const checked = event.target.checked;
            setMergeBoxChecked(checked);
            if (checked) {
                this.palavyrNode.RouteToMostRecentSplitMerge();
            } else {
                // const thing = this.palavyrNode.parentNodeReferences.references[0]
                await this.palavyrNode.addDefaultChild("Continue");
            }
        };

        return () => {
            const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(this.palavyrNode.isPalavyrSplitmergeEnd);
            return shouldShow() ? <NodeCheckBox label="Merge with primary sibling branch" checked={mergeBoxChecked} onChange={(event) => onClick(event, setMergeBoxChecked)} /> : <></>;
        };
    }
}
