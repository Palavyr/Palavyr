import React, { ComponentProps, useState } from "react";
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
import { NodeReferences } from "./PalavyrChildNodes";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { NodeHeader } from "./NodeInterfaceHeader";
import { PalavyrLinkedList } from "./PalavyrLinkedList";
import { _handleMergeBackInOnClick } from "../nodes/nodeInterface/nodeInterfaceCallbacks/_handleMergeBackInOnClick";
import { _handleSetAsAnabranchMergePointClick, setNodeAsAnabranchMergePoint } from "../nodes/nodeInterface/nodeInterfaceCallbacks/_handleSetAsAnabranchMergePointClick";
import { _handleUnsetCurrentNodeType } from "../nodes/nodeInterface/nodeInterfaceCallbacks/_handleUnsetCurrentNodeType";
import { _showResponseInPdfCheckbox } from "../nodes/nodeInterface/nodeInterfaceCallbacks/_showResponseInPdfCheckbox";
import { SteppedLineTo } from "../treeLines/SteppedLineTo";

import "./stylesPalavyrNode.css";

type ComponentType = React.ComponentType<{}>;

interface IPalavyrNode {
    createPalavyrNodeComponent(): ComponentType;
}

export type LineLink = {
    from: string;
    to: string;
};
export type LineMap = LineLink[];

export type lineStyle = {
    borderColor: "white" | string;
    borderStyle: "solid";
    borderWidth: number;
    zIndex: number;
};

export const connectionStyle: lineStyle = {
    borderColor: "#54585A",
    borderStyle: "solid",
    borderWidth: 1,
    zIndex: 0,
};

export abstract class PalavyrNode implements IPalavyrNode {
    // used in widget resource
    public isRoot: boolean;
    public nodeId: string;

    protected isTerminal: boolean;
    protected isMultiOptionType: boolean;
    protected userText: string; // text
    protected shouldPresentResponse: boolean; // isCritical
    protected isDynamicTableNode: boolean;
    protected resolveOrder: number;
    protected nodeType: string; // type of node - e.g. YesNo, Outer-Categories-TwoNestedCategory-fffeefb5-36f2-40cd-96c1-f1eff401393c
    protected nodeComponentType: string; // type of component to use in the widget - standardized list of types in the widget registry
    protected dynamicType: string | null; // generic dynamic type, e.g. SelectOneFlat-3242-2342-234-2423
    public nodeChildrenString: string;

    protected valueOptions: string; // the options available from this node, if any. I none, then "Continue" is used |peg| delimted
    protected optionPath: string; // the value option that was used with the parent of this node.

    // transient
    protected shouldRenderChildren: boolean;
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

    protected rerender: () => void;
    protected repository: PalavyrRepository;
    protected palavyrLinkedList: PalavyrLinkedList; // the containing list object that this node is a member of. Used to acccess update methods

    // deprecated
    protected fallback: boolean;
    protected nodeTypeOptions: NodeTypeOptions;

    /**
     * this node type will hold a reference to the parent nodes
     * We could have multiple parents (e.g. anabranch)
     * We could have no parents if root

     * We hold a reference to the child nodes
     * We could have multiple children
     * We could have no children (if not set)
     **/

    constructor(containerList: PalavyrLinkedList, nodeTypeOptions: NodeTypeOptions, repository: PalavyrRepository, node: ConvoNode, nodeList: ConvoNode[], reRender: () => void, leftmostBranch: boolean) {
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

        this.nodeChildrenString = this.childNodeReferences.childNodeString;

        this.isSplitMergeType = node.isSplitMergeType;
        this.shouldShowMultiOption = node.shouldShowMultiOption;
        this.isAnabranchType = node.isAnabranchType;
        this.isAnabranchMergePoint = node.isAnabranchMergePoint;
        this.isImageNode = node.isImageNode;
        this.imageId = node.imageId;
        this.nodeIdentity = getNodeIdentity(node, nodeList);

        this.isMemberOfLeftmostBranch = leftmostBranch;

        this.rawNode = node;
        this.rawNodeList = nodeList;
        this.rerender = reRender;

        // deprecated
        this.fallback = node.fallback;
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
            nodeChildrenString: this.childNodeReferences.childNodeString,
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
                    {this.lineMap.map((line: LineLink, index: number) => {
                        return <SteppedLineTo key={`${line.to}-${line.from}-stepped-line`} from={line.from} to={line.to} fromAnchor="top" toAnchor="bottom" orientation="v" {...connectionStyle} />;
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

    private renderOptionals() {
        return () => {
            const { setNodes, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData } = React.useContext(ConversationTreeContext);

            const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);
            const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(false);

            const showResponseInPdfCheckbox = (event: { target: { checked: boolean } }) => {
                const checked = event.target.checked;
                _showResponseInPdfCheckbox(checked, this.rawNode, this.rawNodeList, setNodes, conversationHistoryPosition, historyTracker, conversationHistory);
            };

            const handleMergeBackInOnClick = (event: { target: { checked: boolean } }) => {
                const checked = event.target.checked;
                _handleMergeBackInOnClick(checked, this.rawNode, this.rawNodeList, conversationHistoryPosition, historyTracker, conversationHistory, setNodes, setMergeBoxChecked, this.nodeIdentity.nodeIdOfMostRecentSplitMergePrimarySibling);
            };

            const handleSetAsAnabranchMergePointClick = (event: { target: { checked: boolean } }) => {
                const checked = event.target.checked;
                _handleSetAsAnabranchMergePointClick(checked, this.rawNode, this.rawNodeList, this.nodeIdentity.nodeIdOfMostRecentAnabranch, setAnabranchMergeChecked, setNodes);
            };

            const handleUnsetCurrentNodeType = () => {
                _handleUnsetCurrentNodeType(this.rawNode, this.rawNodeList, setNodes);
            };

            return (
                <>
                    {this.nodeIdentity.shouldShowResponseInPdfOption && <NodeCheckBox label="Show response in PDF" checked={this.shouldPresentResponse} onChange={showResponseInPdfCheckbox} />}
                    {this.nodeIdentity.shouldShowMergeWithPrimarySiblingBranchOption && <NodeCheckBox label="Merge with primary sibling branch" checked={!this.shouldRenderChildren} onChange={handleMergeBackInOnClick} />}
                    {this.nodeIdentity.shouldShowSetAsAnabranchMergePointOption && (
                        <NodeCheckBox disabled={this.isAnabranchType && this.isAnabranchMergePoint} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={handleSetAsAnabranchMergePointClick} />
                    )}
                    {this.nodeIdentity.shouldShowUnsetNodeTypeOption && <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={handleUnsetCurrentNodeType} />}
                    {this.nodeIdentity.shouldShowSplitMergePrimarySiblingLabel && <Typography>This is the primary sibling. Branches will merge to this node.</Typography>}
                    {this.nodeIdentity.shouldShowAnabranchMergepointLabel && <Typography style={{ fontWeight: "bolder" }}>This is the Anabranch Merge Node</Typography>}
                </>
            );
        };
    }
}
