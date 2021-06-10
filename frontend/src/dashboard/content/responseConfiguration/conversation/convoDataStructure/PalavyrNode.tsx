import React, { ReactFragment, useCallback, useState } from "react";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { NodeIdentity, ConvoNode, SetState } from "@Palavyr-Types";
import classNames from "classnames";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { Card, CardContent, makeStyles, Typography } from "@material-ui/core";
import { NodeCheckBox } from "../nodes/nodeInterface/NodeCheckBox";
import { DataLogging } from "../nodes/nodeInterface/nodeDebug/DataLogging";
import { ConversationNodeEditor } from "../nodes/nodeInterface/nodeEditor/ConversationNodeEditor";
import { CustomImage } from "../nodes/nodeInterface/nodeEditor/imageNode/CustomImage";
import { NodeTypeSelector } from "../nodes/nodeInterface/nodeSelector/NodeTypeSelector";
import { filteredNodeTypeOptions } from "../nodes/nodeInterface/nodeTypeFilter";
import { getNodeIdentity } from "../nodes/nodeUtils/nodeIdentity";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { NodeReferences } from "./PalavyrChildNodes";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { NodeInterfaceHeader } from "./NodeInterfaceHeader";
import { NodeBody } from "./NodeBody";

interface IPalavyrNode {
    compileDebug(): { [key: string]: string }[];
}

export class PalavyrNode implements IPalavyrNode {
    // used in widget resource
    public isRoot: boolean;
    public nodeId: string;

    private isTerminal: boolean;
    private isMultiOptionType: boolean;
    private userText: string; // text
    private shouldPresentResponse: boolean; // isCritical
    private isDynamicTableNode: boolean;
    private resolveOrder: number;
    private nodeType: string; // type of node - e.g. YesNo, Outer-Categories-TwoNestedCategory-fffeefb5-36f2-40cd-96c1-f1eff401393c
    private nodeComponentType: string; // type of component to use in the widget - standardized list of types in the widget registry
    private dynamicType: string | null; // generic dynamic type, e.g. SelectOneFlat-3242-2342-234-2423
    public nodeChildrenString: string;

    private valueOptions: string; // the options available from this node, if any. I none, then "Continue" is used |peg| delimted
    private optionPath: string; // the value option that was used with the parent of this node.

    // transient
    private shouldRenderChildren: boolean;
    private isSplitMergeType: boolean;
    private shouldShowMultiOption: boolean;
    private isAnabranchType: boolean;
    private isAnabranchMergePoint: boolean;
    private isImageNode: boolean;
    private imageId: string | null;

    // core
    private nodeIdentity: NodeIdentity;
    public childNodeReferences: NodeReferences;
    public parentNodeReferences: NodeReferences;

    public isMemberOfLeftmostBranch: boolean;

    private rerender: () => void;

    // deprecated
    private fallback: boolean;

    /**
     * this node type will hold a reference to the parent nodes
     * We could have multiple parents (e.g. anabranch)
     * We could have no parents if root

     * We hold a reference to the child nodes
     * We could have multiple children
     * We could have no children (if not set)
     */

    constructor(node: ConvoNode, nodeList: ConvoNode[], reRender: () => void, leftmostBranch: boolean) {
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

        this.rerender = reRender;

        // deprecated
        this.fallback = node.fallback;
    }

    public getNextNodes() {
        return this.childNodeReferences;
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

    public compileDebug(): { [key: string]: string }[] {
        // this will return an array of objects that will be used to preset debug data
        const { ...object } = this;
        return Object.keys(object).map((key: string) => {
            return {
                [key]: object[key],
            };
        });
    }

    public renderPalavyrNode() {
        // non jsx
        return () => {
            // jsx
            return (
                <div className={`tree-item tree-item-${this.nodeId}`}>
                    <div className="tree-block-wrap">{this.renderNodeInterface()}</div>
                    {this.childNodeReferences.NotEmpty() && (
                        <div key={this.nodeId} className="tree-row">
                            {this.shouldRenderChildren ? this.childNodeReferences.nodes.map((nextNode: PalavyrNode) => nextNode.renderPalavyrNode()) : <></>}
                        </div>
                    )}
                </div>
            );
        };
    }

    private renderNodeTypeSelector() {
        const { nodeTypeOptions } = React.useContext(ConversationTreeContext);
        return (
            <NodeTypeSelector
                nodeIdentity={this.nodeIdentity}
                nodeTypeOptions={filteredNodeTypeOptions(this.nodeIdentity, nodeTypeOptions)}
                nodeType={this.nodeType}
                reRender={reRender} // this will need to be called from somewhere
                shouldDisabledNodeTypeSelector={this.nodeIdentity.shouldDisabledNodeTypeSelector}
                selectionCallback={selectionCallback} // passed to changeNodeType, and takes care of the anabranch scenario
            />
        );
    }

    private renderImageNodeEditor(): React.ReactNode {
        return () => {
            const [options, setOptions] = useState<Array<string>>([]);
            const [textState, setText] = useState<string>("");
            const [switchState, setSwitchState] = useState<boolean>(true);

            const { nodeList, setNodes } = React.useContext(ConversationTreeContext);

            useEffect(() => {
                setText(this.userText);
                if (node.isMultiOptionType && !isNullOrUndefinedOrWhitespace(node.valueOptions)) {
                    setOptions(node.valueOptions.split(ValueOptionDelimiter));
                }
            }, [node]);

            const handleCloseModal = () => {
                setModalState(false);
            };

            const handleUpdateNode = (value: string, valueOptions: string[]) => {
                const updatedNode = { ...node };
                updatedNode.text = value;
                if (node.isMultiOptionType) {
                    updateMultiTypeOption(updatedNode, nodeList, valueOptions, setNodes); // create new nodes and update the Database
                } else {
                    updateSingleOptionType(updatedNode, nodeList, setNodes);
                }
            };

            const addMultiChoiceOptionsOnClick = () => {
                options.push("");
                setOptions(options);
                setSwitchState(!switchState);
            };

            return (
                <Dialog fullWidth open={modalState} onClose={handleCloseModal} aria-labelledby="form-dialog-title">
                    <DialogTitle id="form-dialog-title">Edit a conversation node</DialogTitle>
                    <DialogContent>
                        {!node.isImageNode && (
                            <>
                                <TextField margin="dense" value={textState} multiline rows={4} onChange={(event) => setText(event.target.value)} id="question" label="Question or Information" type="text" fullWidth />
                                {node.isMultiOptionType && node.shouldShowMultiOption && (
                                    <>
                                        <MultiChoiceOptions options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} addMultiChoiceOptionsOnClick={addMultiChoiceOptionsOnClick} />
                                    </>
                                )}
                            </>
                        )}
                        {node.isImageNode && (
                            <>
                                {node.imageId === null ? (
                                    <>
                                        <Typography align="center" variant="h6">
                                            Upload an image
                                        </Typography>
                                        <NodeImageUpload setModalState={setModalState} node={node} currentImageId={currentImageId} setImageLink={setImageLink} setImageName={setImageName} />
                                    </>
                                ) : (
                                    <>
                                        <CustomImage imageName={imageName} imageLink={imageLink} />
                                        <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                                        <Typography align="center" variant="h6">
                                            Choose a new image
                                        </Typography>
                                        <NodeImageUpload setModalState={setModalState} node={node} currentImageId={currentImageId} setImageLink={setImageLink} setImageName={setImageName} initialState={false} />
                                        <Divider />
                                    </>
                                )}
                            </>
                        )}
                    </DialogContent>
                    {!node.isImageNode && (
                        <DialogActions>
                            <SaveOrCancel
                                position="right"
                                customSaveMessage="Node Text Updated"
                                customCancelMessage="Changes cancelled"
                                useSaveIcon={false}
                                saveText="Update Node Text"
                                onSave={async () => {
                                    handleUpdateNode(textState, options);
                                    handleCloseModal();
                                    return true;
                                }}
                                onCancel={handleCloseModal}
                                timeout={200}
                            />
                        </DialogActions>
                    )}
                </Dialog>
            );
        };
    }

    public renderNodeInterface(): React.ReactNode {
        return () => {
            const repository = new PalavyrRepository();
            const { setNodes, nodeList, nodeTypeOptions, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData } = React.useContext(ConversationTreeContext);

            const [modalState, setModalState] = useState<boolean>(false);
            const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(false);
            const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);

            const cls = useNodeInterfaceStyles({
                nodeType: this.nodeType,
                nodeText: this.userText,
                checked: this.shouldPresentResponse,
                isDecendentOfSplitMerge: this.nodeIdentity.isDecendentOfSplitMerge,
                splitMergeRootSiblingIndex: this.nodeIdentity.splitMergeRootSiblingIndex,
                debugOn: showDebugData,
                isImageNode: this.imageId !== null,
            });

            return (
                <Card className={classNames(cls.root, this.nodeId)} variant="outlined">
                    <CardContent className={cls.card}>
                        {showDebugData && <DataLogging debugData={this.compileDebug()} nodeChildren={this.nodeChildrenString} nodeId={this.nodeId} />}
                        <NodeInterfaceHeader isRoot={this.isRoot} optionPath={this.optionPath} />
                        {this.isImageNode ? this.renderImageNodeFace(setModalState) : this.renderNodeFace(setModalState)}
                        {this.renderNodeTypeSelector()}
                        {this.nodeIdentity.shouldShowResponseInPdfOption && <NodeCheckBox label="Show response in PDF" checked={this.shouldPresentResponse} onChange={showResponseInPdfCheckbox} />}
                        {this.nodeIdentity.shouldShowMergeWithPrimarySiblingBranchOption && <NodeCheckBox label="Merge with primary sibling branch" checked={!this.shouldRenderChildren} onChange={handleMergeBackInOnClick} />}
                        {this.nodeIdentity.shouldShowSetAsAnabranchMergePointOption && (
                            <NodeCheckBox disabled={this.isAnabranchType && this.isAnabranchMergePoint} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={handleSetAsAnabranchMergePointClick} />
                        )}
                        {this.nodeIdentity.shouldShowUnsetNodeTypeOption && <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={handleUnsetCurrentNodeType} />}
                        {this.nodeIdentity.shouldShowSplitMergePrimarySiblingLabel && <Typography>This is the primary sibling. Branches will merge to this node.</Typography>}
                        {this.nodeIdentity.shouldShowAnabranchMergepointLabel && <Typography style={{ fontWeight: "bolder" }}>This is the Anabranch Merge Node</Typography>}
                    </CardContent>
                </Card>
            );
        };
    }

    private renderImageNodeFace(setModalState: SetState<boolean>) {
        const cls = useNodeInterfaceStyles();

        const [imageLink, setImageLink] = useState<string>("");
        const [imageName, setImageName] = useState<string>("");
        const [currentImageId, setCurrentImageId] = useState<string>("");

        const loadImage = useCallback(async () => {
            if (this.imageId !== null && this.imageId !== undefined) {
                const fileLinks = await repository.Configuration.Images.getImages([this.imageId]);
                const fileLink = fileLinks[0];
                if (!fileLink.isUrl) {
                    const presignedUrl = await repository.Configuration.Images.getSignedUrl(fileLink.link);
                    setImageLink(presignedUrl);
                    setImageName(fileLink.fileName);
                    setCurrentImageId(fileLink.fileId);
                }
            }
        }, [nodeList]);

        return () => {
            return (
                <NodeBody>
                    <CustomImage imageName={imageName} imageLink={imageLink} titleVariant="body1" />
                </NodeBody>
            );
        };
    }

    private renderNodeFace(setModalState: SetState<boolean>) {
        const cls = useNodeInterfaceStyles();
        return () => {
            return (
                <NodeBody>
                    <Typography className={cls.text} variant="body2" component="span" noWrap={false}>
                        {this.userText}
                    </Typography>
                </NodeBody>
            );
        };
    }
}
