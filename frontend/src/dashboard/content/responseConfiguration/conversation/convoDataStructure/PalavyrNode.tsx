import React, { useState } from "react";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { NodeIdentity, ConvoNode, SetState, Conversation, NodeTypeOptions } from "@Palavyr-Types";
import classNames from "classnames";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { Card, CardContent, Typography } from "@material-ui/core";
import { NodeCheckBox } from "../nodes/nodeInterface/NodeCheckBox";
import { DataLogging } from "../nodes/nodeInterface/nodeDebug/DataLogging";
import { NodeTypeSelector } from "../nodes/nodeInterface/nodeSelector/NodeTypeSelector";
import { filteredNodeTypeOptions } from "../nodes/nodeInterface/nodeTypeFilter";
import { getNodeIdentity } from "../nodes/nodeUtils/nodeIdentity";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { NodeReferences } from "./NodeReferences";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { NodeHeader } from "./NodeInterfaceHeader";
import { PalavyrLinkedList } from "./PalavyrLinkedList";
import { _handleMergeBackInOnClick } from "../nodes/nodeInterface/nodeInterfaceCallbacks/_handleMergeBackInOnClick";
import { _handleSetAsAnabranchMergePointClick, setNodeAsAnabranchMergePoint } from "../nodes/nodeInterface/nodeInterfaceCallbacks/_handleSetAsAnabranchMergePointClick";
import { _handleUnsetCurrentNodeType } from "../nodes/nodeInterface/nodeInterfaceCallbacks/_handleUnsetCurrentNodeType";
import { _showResponseInPdfCheckbox } from "../nodes/nodeInterface/nodeInterfaceCallbacks/_showResponseInPdfCheckbox";
import { SteppedLineTo } from "../treeLines/SteppedLineTo";

import "./stylesPalavyrNode.css";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";

type ComponentType = React.ComponentType<{}>;

interface IPalavyrNode {
    createPalavyrNodeComponent(): ComponentType;
}

export type LineLink = {
    from: string;
    to: string;
};
export type LineMap = LineLink[];

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

    protected valueOptions: string; // the options available from this node, if any. I none, then "Continue" is used |peg| delimted
    public optionPath: string; // the value option that was used with the parent of this node.

    // transient
    public shouldRenderChildren: boolean;
    protected isSplitMergeType: boolean;
    protected shouldShowMultiOption: boolean;
    protected isAnabranchType: boolean;
    protected isAnabranchMergePoint: boolean;
    protected isImageNode: boolean;
    protected imageId: string | null;

    // core
    protected nodeIdentity: NodeIdentity;
    public childNodeReferences: NodeReferences;
    public parentNodeReferences: NodeReferences;

    public isMemberOfLeftmostBranch: boolean;

    public rawNode: ConvoNode; // Get Rid Of This.
    public rawNodeList: Conversation;

    public lineMap: LineMap = [];

    public setTreeWithHistory: (updatedTree: PalavyrLinkedList) => void;
    protected repository: PalavyrRepository;
    public palavyrLinkedList: PalavyrLinkedList; // the containing list object that this node is a member of. Used to acccess update methods

    // deprecated
    protected fallback: boolean;
    protected nodeTypeOptions: NodeTypeOptions;

    // NEW PROPERTIES TO DEAL WITH anabranch and split merge types --

    // ANA BRANCH (we get isAnabranch from the DB. We should infer the rest here when constructing the linked list)
    public isPalavyrAnabranchStart: boolean;
    public isPalavyrAnabranchMember: boolean;
    public isPalavyrAnabranchEnd: boolean;

    // SPLIT MERGE (we also get isSplitMerge from Db, so samesy - infer the rest here)
    public isPalavyrSplitmergeStart: boolean;
    public isPalavyrSplitmergeMember: boolean;
    public isPalavyrSplitmergePrimarybranch: boolean;
    public isPalavyySplitmergeEnd: boolean;
    public isPalavyrSplitMergeMergePoint: boolean;

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

        this.childNodeReferences = new NodeReferences();
        this.parentNodeReferences = new NodeReferences();

        this.optionPath = node.optionPath; // the value option that was used with the parent of this node.
        this.valueOptions = node.valueOptions; // the options available from this node, if any. I none, then "Continue" is used |peg| delimted

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
        this.isPalavyrSplitMergeMergePoint = node.IsSplitMergeMergePoint;

        this.isAnabranchType = node.isAnabranchType;
        this.isAnabranchMergePoint = node.isAnabranchMergePoint;
        this.isImageNode = node.isImageNode;
        this.imageId = node.imageId;
        this.nodeIdentity = getNodeIdentity(node, nodeList);

        this.isMemberOfLeftmostBranch = leftmostBranch;

        this.rawNode = node;
        this.rawNodeList = nodeList;
        this.setTreeWithHistory = setTreeWithHistory;

        // deprecated
        this.fallback = node.fallback;
    }

    public UpdateTree() {
        this.setTreeWithHistory(this.palavyrLinkedList);
    }

    public configure(parentNode: PalavyrNode) {
        this.parentNodeReferences.addReference(parentNode);
        this.addLine(parentNode.nodeId);
        this.configureAnabranch(parentNode);
        this.configureSplitMerge(parentNode);
    }

    private configureAnabranch(parentNode: PalavyrNode) {
        this.isPalavyrAnabranchStart = this.isAnabranchType;
        this.isPalavyrAnabranchMember = parentNode.isAnabranchType || (parentNode.isPalavyrAnabranchMember && !parentNode.isPalavyrAnabranchEnd);
        this.isPalavyrAnabranchEnd = this.isAnabranchMergePoint;
    }

    private configureSplitMerge(parentNode: PalavyrNode) {
        this.isPalavyrSplitmergeStart = this.isSplitMergeType;
        this.isPalavyrSplitmergeMember = parentNode.isSplitMergeType || (parentNode.isPalavyrSplitmergeMember && !parentNode.isPalavyySplitmergeEnd);
        this.isPalavyrSplitmergePrimarybranch = this.isMemberOfLeftmostBranch;
        this.isPalavyySplitmergeEnd = this.isPalavyrSplitMergeMergePoint;
    }

    public sortChildReferences() {
        // reorder parent's child refs depending on if parent is a anabranch or splitmerge type
        if (!this.isAnabranchType && !this.isSplitMergeType) {
            this.childNodeReferences.OrderByOptionPath();
        }
    }

    public async addDefaultChild() {
        const defaultNode = await this.repository.Conversations.GetDefaultConversationNode();
        this.addNewNodeReference(defaultNode, this);
        this.UpdateTree();
    }

    public addNewNodeReference(newNode: PalavyrNode, parentNode: PalavyrNode) {
        // double linked
        parentNode.childNodeReferences.addReference(newNode);
        newNode.configure(parentNode);
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
            valueOptions: this.valueOptions,
            isDynamicTableNode: this.isDynamicTableNode,
            dynamicType: this.dynamicType,
            resolveOrder: this.resolveOrder,
            isTerminalType: this.isTerminal,
            shouldRenderChildren: this.shouldRenderChildren,
            isMultiOptionType: this.isMultiOptionType,
            fallback: this.fallback,
            isSplitMergeType: this.isSplitMergeType,
            IsSplitMergeMergePoint: this.isPalavyrSplitMergeMergePoint,
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

    private renderNodeTypeSelector(selectionCallback: (node: ConvoNode, nodeList: Conversation, nodeIdOfMostRecentAnabranch: string) => void) {
        return () => {
            return (
                <NodeTypeSelector
                    nodeIdentity={this.nodeIdentity}
                    nodeTypeOptions={filteredNodeTypeOptions(this.nodeIdentity, this.nodeTypeOptions)}
                    nodeType={this.nodeType}
                    reRender={this.rerender} // this will need to be called from somewhere
                    shouldDisabledNodeTypeSelector={this.nodeIdentity.shouldDisabledNodeTypeSelector}
                    selectionCallback={selectionCallback} // passed to changeNodeType, and takes care of the anabranch scenario
                />
            );
        };
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
                isDecendentOfSplitMerge: this.nodeIdentity.isDecendentOfSplitMerge,
                splitMergeRootSiblingIndex: this.nodeIdentity.splitMergeRootSiblingIndex,
                debugOn: showDebugData,
                isImageNode: this.imageId !== null,
            });

            const selectionCallback = (node: ConvoNode, nodeList: Conversation, nodeIdOfMostRecentAnabranch: string): void => {
                // return setNodeAsAnabranchMergePoint(node, nodeList, nodeIdOfMostRecentAnabranch, setAnabranchMergeChecked);
            };

            return (
                <Card className={classNames(cls.root, this.nodeId)} variant="outlined">
                    <CardContent className={cls.card}>
                        {showDebugData && <DataLogging debugData={this.compileDebug()} nodeChildren={this.nodeChildrenString} nodeId={this.nodeId} />}
                        <NodeHeader isRoot={this.isRoot} optionPath={this.optionPath} />
                        {this.renderNodeFace()({ openEditor })}
                        {this.renderNodeTypeSelector(selectionCallback)()}
                        {this.renderNodeEditor()({ editorIsOpen, closeEditor })}
                        {this.renderOptionals()()}
                    </CardContent>
                </Card>
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

    private renderOptionals() {
        const nodeOptionals = new PalavyrNodeOptionals(this);

        return () => {
            const { setNodes, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData } = React.useContext(ConversationTreeContext);

            const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);
            // const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(false);

            // const showResponseInPdfCheckbox = (event: { target: { checked: boolean } }) => {
            //     const checked = event.target.checked;
            //     _showResponseInPdfCheckbox(checked, this.rawNode, this.rawNodeList, setNodes, conversationHistoryPosition, historyTracker, conversationHistory);
            // };

            // const handleMergeBackInOnClick = (event: { target: { checked: boolean } }) => {
            //     const checked = event.target.checked;
            //     _handleMergeBackInOnClick(
            //         checked,
            //         this.rawNode,
            //         this.rawNodeList,
            //         conversationHistoryPosition,
            //         historyTracker,
            //         conversationHistory,
            //         setNodes,
            //         setMergeBoxChecked,
            //         this.nodeIdentity.nodeIdOfMostRecentSplitMergePrimarySibling
            //     );
            // };

            const handleSetAsAnabranchMergePointClick = (event: { target: { checked: boolean } }) => {
                const checked = event.target.checked;
                _handleSetAsAnabranchMergePointClick(checked, this.rawNode, this.rawNodeList, this.nodeIdentity.nodeIdOfMostRecentAnabranch, setAnabranchMergeChecked, setNodes);
            };

            const handleUnsetCurrentNodeType = () => {
                _handleUnsetCurrentNodeType(this.rawNode, this.rawNodeList, setNodes);
            };

            return (
                <>
                    {nodeOptionals.renderShowResponseInPdf()()}
                    {/* {this.shouldShowResponseInPdfOption() && <NodeCheckBox label="Show response in PDF" checked={this.shouldPresentResponse} onChange={showResponseInPdfCheckbox} />} */}

                    {/* {this.shouldShowMergeWithPrimarySiblingBranchOption() && <NodeCheckBox label="Merge with primary sibling branch" checked={!this.shouldRenderChildren} onChange={handleMergeBackInOnClick} />} */}

                    {this.nodeIdentity.shouldShowSetAsAnabranchMergePointOption && (
                        <NodeCheckBox
                            disabled={this.isAnabranchType && this.isAnabranchMergePoint}
                            label="Set as Anabranch merge point"
                            checked={anabranchMergeChecked}
                            onChange={handleSetAsAnabranchMergePointClick}
                        />
                    )}

                    {this.nodeIdentity.shouldShowUnsetNodeTypeOption && <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={handleUnsetCurrentNodeType} />}

                    {this.nodeIdentity.shouldShowSplitMergePrimarySiblingLabel && <Typography>This is the primary sibling. Branches will merge to this node.</Typography>}

                    {this.nodeIdentity.shouldShowAnabranchMergepointLabel && <Typography style={{ fontWeight: "bolder" }}>This is the Anabranch Merge Node</Typography>}
                </>
            );
        };
    }

    // private shouldShowResponseInPdfOption() {
    //     const nodeTypesThatDoNotProvideFeedback = ["ProvideInfo"];
    //     return !this.isTerminal && !nodeTypesThatDoNotProvideFeedback.includes(this.nodeType);
    // }

    // private shouldShowMergeWithPrimarySiblingBranchOption() {
    //     return this.isPalavyrSplitmergeMember && this.isPalavyrSplitmergePrimarybranch && this.nodeTypeIsSet() && !this.isTerminal && !this.isMultiOptionType && this.isPenultimate();
    // }

    // private nodeTypeIsSet() {
    //     return !isNullOrUndefinedOrWhitespace(this.nodeType);
    // }

    private isPenultimate() {
        if (this.childNodeReferences.Empty()) return false;

        for (let index = 0; index < this.childNodeReferences.getPalavyrNodesReferences.length; index++) {
            const childNodeReference = this.childNodeReferences.getPalavyrNodesReferences[index];
            if (childNodeReference.childNodeReferences.getPalavyrNodesReferences.length !== 0) {
                return false; // check this logic TODO:!
            }
            return true;
        }
    }
}

export type OnChangeBooleanCallback = (event: { target: { checked: boolean } }) => void;

export class PalavyrNodeOptionals {
    private palavyrNode: PalavyrNode;
    constructor(node: PalavyrNode) {
        this.palavyrNode = node;
    }

    public renderShowResponseInPdf() {
        const shouldShowResponseInPdfOption = () => {
            const nodeTypesThatDoNotProvideFeedback = ["ProvideInfo"];
            return !this.palavyrNode.isTerminal && !nodeTypesThatDoNotProvideFeedback.includes(this.palavyrNode.nodeType);
        };
        const toggleShowResponse = (event: { target: { checked: boolean } }) => {
            const checked = event.target.checked;
            this.palavyrNode.shouldPresentResponse = checked;
            this.palavyrNode.setTreeWithHistory(this.palavyrNode.palavyrLinkedList);
        };

        return () => {
            return shouldShowResponseInPdfOption() ? <NodeCheckBox label="Show response in PDF" checked={this.palavyrNode.shouldPresentResponse} onChange={toggleShowResponse} /> : <></>;
        };
    }

    public renderShowMergeWithPrimarySiblingBranchOption() {
        const shouldShowMergeWithPrimarySiblingBranchOption = () => {
            return (
                this.palavyrNode.isPalavyrSplitmergeMember &&
                this.palavyrNode.isPalavyrSplitmergePrimarybranch &&
                this.nodeTypeIsSet() &&
                !this.palavyrNode.isTerminal &&
                !this.palavyrNode.isMultiOptionType &&
                this.isPenultimate()
            );
        };

        const handleMergeBackInOnClick = async (event: { target: { checked: boolean } }, setMergeBoxChecked: SetState<boolean>) => {
            const checked = event.target.checked;
            setMergeBoxChecked(checked);
            if (checked) {
                this.palavyrNode.RouteToMostRecentSplitMerge();
            } else {

                await this.palavyrNode.addDefaultChild();

                // if (conversationHistoryPosition === 0) {
                //     const childId = uuid();
                //     const newNode = cloneDeep(node);
                //     newNode.shouldRenderChildren = true;
                //     newNode.nodeChildrenString = childId;
                //     let updatedNodeList = _createAndAddNewNodes([childId], [childId], node.areaIdentifier, ["Continue"], nodeList, false, false);
                //     // updateSingleOptionType(newNode, updatedNodeList, setNodes);
                //     setMergeBoxChecked(false);
                // } else {
                //     historyTracker.stepConversationBackOneStep(conversationHistoryPosition, conversationHistory);
                // }
            }
        };

        return () => {
            const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(false);
            return shouldShowMergeWithPrimarySiblingBranchOption() ? (
                <NodeCheckBox label="Merge with primary sibling branch" checked={mergeBoxChecked} onChange={(event) => handleMergeBackInOnClick(event, setMergeBoxChecked)} />
            ) : (
                <></>
            );
        };
    }

    private nodeTypeIsSet() {
        return !isNullOrUndefinedOrWhitespace(this.palavyrNode.nodeType);
    }

    private isPenultimate() {
        if (this.palavyrNode.childNodeReferences.Empty()) return false;

        for (let index = 0; index < this.palavyrNode.childNodeReferences.getPalavyrNodesReferences.length; index++) {
            const childNodeReference = this.palavyrNode.childNodeReferences.getPalavyrNodesReferences[index];
            if (childNodeReference.childNodeReferences.getPalavyrNodesReferences.length !== 0) {
                return false; // check this logic TODO:!
            }
            return true;
        }
    }
}
