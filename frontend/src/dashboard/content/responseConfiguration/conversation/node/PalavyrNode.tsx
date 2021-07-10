import React, { useState, useEffect, useContext, memo } from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { Card, CardContent, makeStyles, Typography } from "@material-ui/core";
import { ConvoNode, NodeTypeOptions, ValueOptionDelimiter, AlertType, NodeOption, LineMap, AnabranchContext, LineLink, NodeTypeCode, LoopbackContext } from "@Palavyr-Types";
import classNames from "classnames";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { DataLogging } from "./nodeInterface/nodeDebug/DataLogging";
import { CustomNodeSelect } from "./nodeInterface/nodeSelector/CustomNodeSelect";
import { SteppedLineTo } from "../PalavyrNodeLines/SteppedLines";
import { useNodeInterfaceStyles } from "../nodeInterfaceStyles";
import { INodeReferences, IPalavyrLinkedList, IPalavyrNode } from "../Contracts";
import { NodeReferences } from "./PalavyrNodeReferences";
import { AnabranchMergeCheckBox } from "./options/AnabranchMergeCheckBox";
import NodeTypeOptionConfigurer from "./actions/NodeTypeOptionConfigurer";
import { PalavyrNodeChanger } from "./actions/NodeChanger";
import { NodeConfigurer } from "./actions/NodeConfigurer";
import { NodeCreator } from "./actions/NodeCreator";
import { AnabranchMergeNodeLabel } from "./options/AnabranchMergeNodeLabel";
import { ShowResponseInPdf } from "./options/ShowResponseInPdf";
import { UnsetNodeButton } from "./options/UnsetNodeButton";
const treelinkClassName = "tree-line-link";

export abstract class PalavyrNodeBase implements IPalavyrNode {
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

    public isLoopbackMember: boolean; // only nonleftmostbranch
    public isLoopbackStart: boolean;
    public loopbackContext: LoopbackContext;

    /**
     * this node type will hold a reference to the parent nodes
     * We could have multiple parents (e.g. anabranch)
     * We could have no parents if root

     * We hold a reference to the child nodes
     * We could have multiple children
     * We could have no children (if not set)
     **/

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

    abstract renderNodeEditor(): ({ editorIsOpen, closeEditor }) => JSX.Element;
    abstract renderNodeFace(): ({ openEditor }) => JSX.Element;

    public UpdateTree() {
        this.setTreeWithHistory(this.palavyrLinkedList);
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
    public createPalavyrNodeComponent(pBuffer: number) {
        type StyleProps = {
            buffer: number;
        };

        const useStyles = makeStyles((theme) => ({
            treeItem: {
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                zIndex: 10,
            },
            treeBlockWrap: (props: StyleProps) => ({
                padding: `${props.buffer}rem ${props.buffer}rem ${props.buffer}rem ${props.buffer}rem`,
            }),
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

            const cls = useStyles({ buffer: pBuffer });
            return (
                <>
                    <div className={classNames(treelinkClassName, cls.treeItem)}>
                        <div className={cls.treeBlockWrap}>{this.renderNodeInterface()()}</div>
                        {this.childNodeReferences.NotEmpty() && (
                            <div key={this.nodeId} className={cls.treeRow}>
                                {this.shouldRenderChildren ? (
                                    this.childNodeReferences.nodes.map(
                                        (nextNode: IPalavyrNode, index: number): React.ReactNode => {
                                            const Node = nextNode.createPalavyrNodeComponent(pBuffer);
                                            return <Node key={[this.nodeId, nextNode.nodeId, index.toString()].join("-")} />;
                                        }
                                    )
                                ) : (
                                    <></>
                                )}
                            </div>
                        )}
                    </div>
                    {loaded &&
                        this.lineMap.map((line: LineLink, index: number) => {
                            return <SteppedLineTo key={[line.from, index].join("-")} from={line.from} to={line.to} treeLinkClassName={treelinkClassName} />;
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
        const nodeChanger = new PalavyrNodeChanger();

        return () => {
            const [alertState, setAlertState] = useState<boolean>(false);
            const [alertDetails, setAlertDetails] = useState<AlertType>();
            const [label, setLabel] = useState<string>("");

            const { nodeTypeOptions } = useContext(ConversationTreeContext);

            useEffect(() => {
                const currentNodeOption = nodeTypeOptions.filter((option: NodeOption) => option.value === this.nodeType)[0];
                if (currentNodeOption) {
                    setLabel(currentNodeOption.text);
                }
            }, [this.nodeType]);

            const duplicateDynamicFeeNodeFound = (option: string, nodeTypeOptions: NodeTypeOptions) => {
                const dynamicNodeTypeOptions = nodeTypeOptions.filter((x: NodeOption) => x.isDynamicType);
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

                if (duplicateDynamicFeeNodeFound(nodeOption.value, nodeTypeOptions)) {
                    setAlertDetails({
                        title: `You've already placed dynamic table ${nodeOption.text} in this conversation`,
                        message:
                            "You can only place each dynamic table in your conversation once. If you would like to change where you've placed it in the conversation, you need to recreate that portion of the tree by selection a different node.",
                    });
                    setAlertState(true);
                    return;
                }

                const updatedNode = this as IPalavyrNode;
                nodeChanger.ExecuteNodeSelectorUpdate(nodeOption, updatedNode, nodeTypeOptions);
            };

            const currentNode = this as IPalavyrNode;
            return (
                <>
                    {nodeTypeOptions && (
                        <CustomNodeSelect
                            onChange={autocompleteOnChange}
                            label={label}
                            nodeTypeOptions={NodeTypeOptionConfigurer.ConfigureNodeTypeOptions(currentNode, nodeTypeOptions)}
                            shouldDisabledNodeTypeSelector={this.shouldDisableNodeTypeSelector}
                        />
                    )}
                    {alertDetails && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />}
                </>
            );
        };
    }

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
                splitMergeRootSiblingIndex: this.isMemberOfLeftmostBranch ? 0 : 1,
                debugOn: showDebugData,
                isImageNode: this.imageId !== null,
            });

            return (
                <Card style={{ border: "5px solid black" }} id={this.nodeId} className={cls.root} variant="outlined">
                    <CardContent className={classNames(cls.card, this.nodeId)}>
                        {showDebugData && <DataLogging debugData={this.compileDebug()} nodeChildren={this.childNodeReferences.joinedReferenceString} nodeId={this.nodeId} />}
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
                    // node.unlock();
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

    private renderOptionals() {
        const currentNode = this as IPalavyrNode;

        return () => {
            return (
                <>
                    <ShowResponseInPdf node={currentNode} />
                    <AnabranchMergeCheckBox node={currentNode} />
                    <UnsetNodeButton node={currentNode} />
                    <AnabranchMergeNodeLabel node={currentNode} />
                </>
            );
        };
    }
}
