import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { CardContent } from "@material-ui/core";
import { Card } from "@material-ui/core";
import classNames from "classnames";
import { ConversationTreeContext, DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useState } from "react";
import { useContext } from "react";
import { IPalavyrNode } from "@Palavyr-Types";
import { useNodeInterfaceStyles } from "../../nodeInterfaceStyles";
import { FileAssetNodeEditor } from "../imageNode/FileAssetNodeEditor";
import { FileAssetNodeFace } from "../imageNode/FileAssetNodeFace";
import { DataLogging } from "../nodeInterface/nodeDebug/DataLogging";
import { TextNodeEditor } from "../textNode/TextNodeEditor";
import { TextNodeFace } from "../textNode/TextNodeFace";
import { NodeHeader } from "./NodeHeader";
import { NodeOptionals } from "./NodeOptionals";
import { NodeTypeSelector } from "./NodeTypeSelector";
import { PositionSwitcher } from "./PositionSwitch";

export interface NodeInterfaceProps {
    currentNode: IPalavyrNode;
    isRoot: boolean;
    nodeType: string;
    userText: string;
    shouldPresentResponse: boolean;
    isMemberOfLeftmostBranch: boolean;
    nodeId: string;
    joinedChildReferenceString: string;
    shouldDisableNodeTypeSelector: boolean;
    optionPath: string;
    repository?: PalavyrRepository;
}

const compileDebug = (currentNode: IPalavyrNode): { [key: string]: string }[] => {
    // this will return an array of objects that will be used to preset debug data
    const { ...object } = currentNode;
    return Object.keys(object).map((key: string) => {
        return {
            [key]: object[key],
        };
    });
};

export const NodeInterface = ({
    currentNode,
    isRoot,
    nodeType,
    userText,
    shouldPresentResponse,
    isMemberOfLeftmostBranch,
    nodeId,
    joinedChildReferenceString,
    shouldDisableNodeTypeSelector,
    optionPath,
}: NodeInterfaceProps) => {
    const { showDebugData } = useContext(ConversationTreeContext);
    const { repository } = useContext(DashboardContext);
    const [editorIsOpen, setEditorState] = useState<boolean>(false);
    const openEditor = () => setEditorState(true);
    const closeEditor = () => setEditorState(false);

    const cls = useNodeInterfaceStyles({
        nodeType: nodeType,
        nodeText: userText,
        checked: shouldPresentResponse,
        splitMergeRootSiblingIndex: isMemberOfLeftmostBranch ? 0 : 1,
        debugOn: showDebugData,
    });

    return (
        <Card id={nodeId} className={cls.root} variant={nodeType === "" ? "outlined" : undefined}>
            <PositionSwitcher currentNode={currentNode} />
            <CardContent className={classNames(cls.card, nodeId)}>
                {showDebugData && <DataLogging debugData={compileDebug(currentNode)} nodeChildren={joinedChildReferenceString} nodeId={nodeId} />}
                <NodeHeader isRoot={isRoot} optionPath={optionPath} nodeId={currentNode.nodeId} />
                {currentNode.isImageNode ? <FileAssetNodeFace fileAssetId={currentNode.fileId} repository={repository} openEditor={openEditor} /> : <TextNodeFace openEditor={openEditor} userText={userText} />}
                <NodeTypeSelector currentNode={currentNode} shouldDisableNodeTypeSelector={shouldDisableNodeTypeSelector} />
                {currentNode.isImageNode ? (
                    <FileAssetNodeEditor currentNode={currentNode} repository={repository} editorIsOpen={editorIsOpen} closeEditor={closeEditor} />
                ) : (
                    <TextNodeEditor
                        isMultiOptionType={currentNode.isMultiOptionType}
                        shouldShowMultiOption={currentNode.shouldShowMultiOption}
                        isAnabranchLocked={currentNode.isAnabranchLocked}
                        currentNode={currentNode}
                        childNodeReferences={currentNode.childNodeReferences}
                        userText={userText}
                        editorIsOpen={editorIsOpen}
                        closeEditor={closeEditor}
                    />
                )}
                <NodeOptionals currentNode={currentNode} />
            </CardContent>
        </Card>
    );
};
