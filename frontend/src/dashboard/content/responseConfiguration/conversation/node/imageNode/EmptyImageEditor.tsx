import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { Typography } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React from "react";
import { IPalavyrNode } from "../../Contracts";
import { ImageUpload } from "./ImageUpload";

export interface IRenderImageEditorWhenEmptyProps {
    currentNode: IPalavyrNode;
    setReload: () => void;
    nodeId: string;
    repository: PalavyrRepository;
    closeEditor: () => void;
    currentImageId: string;
    setImageLink: SetState<string>;
    setImageName: SetState<string>;
}

export const EmptyImageEditor = ({ currentNode, setReload, nodeId, repository, closeEditor, currentImageId, setImageLink, setImageName }: IRenderImageEditorWhenEmptyProps) => {
    return (
        <>
            <Typography align="center" variant="h6">
                Upload an image
            </Typography>
            {/* <ImageUpload
                updateTree={() => null)
                currentNode={currentNode}
                setReload={setReload}
                nodeId={nodeId}
                closeEditor={closeEditor}
                currentImageId={currentImageId}
                setImageLink={setImageLink}
                setImageName={setImageName}
                repository={repository}
                initialState={false}
            /> */}
        </>
    );
};
