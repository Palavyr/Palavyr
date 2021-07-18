import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { CardContent } from "@material-ui/core";
import { Card } from "@material-ui/core";
import classNames from "classnames";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import React, { useState } from "react";
import { IPalavyrNode } from "../../Contracts";
import { useNodeInterfaceStyles } from "../../nodeInterfaceStyles";
import { ImageNodeEditor } from "../imageNode/ImageNodeEditor";
import { ImageNodeFace } from "../imageNode/ImageNodeFace";
import { DataLogging } from "../nodeInterface/nodeDebug/DataLogging";
import { TextNodeEditor } from "../textNode/TextNodeEditor";
import { TextNodeFace } from "../textNode/TextNodeFace";
import { NodeHeader } from "./NodeHeader";
import { NodeOptionals } from "./NodeOptionals";
import { NodeTypeSelector } from "./NodeTypeSelector";

export interface NodeInterfaceProps {
    currentNode: IPalavyrNode;
    isRoot: boolean;
    nodeType: string;
    userText: string;
    shouldPresentResponse: boolean;
    isMemberOfLeftmostBranch: boolean;
    imageId?: string | null;
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
    imageId,
    nodeId,
    joinedChildReferenceString,
    shouldDisableNodeTypeSelector,
    optionPath,
}: NodeInterfaceProps) => {
    const { showDebugData } = React.useContext(ConversationTreeContext);
    const repository = new PalavyrRepository();
    const [editorIsOpen, setEditorState] = useState<boolean>(false);
    const openEditor = () => setEditorState(true);
    const closeEditor = () => setEditorState(false);

    const cls = useNodeInterfaceStyles({
        nodeType: nodeType,
        nodeText: userText,
        checked: shouldPresentResponse,
        splitMergeRootSiblingIndex: isMemberOfLeftmostBranch ? 0 : 1,
        debugOn: showDebugData,
        // isImageNode: currentNode.isImageNode, //imageId !== undefined|| imageId === null
    });

    return (
        <Card id={nodeId} className={cls.root} variant={nodeType === "" ? "outlined" : undefined}>
            <CardContent className={classNames(cls.card, nodeId)}>
                {showDebugData && <DataLogging debugData={compileDebug(currentNode)} nodeChildren={joinedChildReferenceString} nodeId={nodeId} />}
                <NodeHeader isRoot={isRoot} optionPath={optionPath} nodeId={currentNode.nodeId} />
                {currentNode.isImageNode ? <ImageNodeFace imageId={imageId} repository={repository} openEditor={openEditor} /> : <TextNodeFace openEditor={openEditor} userText={userText} />}
                <NodeTypeSelector currentNode={currentNode} shouldDisableNodeTypeSelector={shouldDisableNodeTypeSelector} />
                {currentNode.isImageNode ? (
                    <ImageNodeEditor currentNode={currentNode} nodeId={nodeId} repository={repository} editorIsOpen={editorIsOpen} closeEditor={closeEditor} imageId={imageId} />
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
