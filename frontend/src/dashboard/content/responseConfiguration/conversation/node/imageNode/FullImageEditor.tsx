import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { Divider, Typography } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React from "react";
import { IPalavyrNode } from "../../Contracts";
import { CustomImage } from "./CustomImage";
import { ImageUpload } from "./ImageUpload";

export interface ImageEditorWhenFullProps {
    currentNode: IPalavyrNode;
    setReload: () => void;
    nodeId: string;
    repository: PalavyrRepository;
    closeEditor: () => void;
    imageName: string;
    imageLink: string;
    currentImageId: string;
    setImageLink: SetState<string>;
    setImageName: SetState<string>;
}

export const FullImageEditor = ({ currentNode, setReload, nodeId, repository, closeEditor, imageName, imageLink, currentImageId, setImageLink, setImageName }: ImageEditorWhenFullProps) => {
    return (
        <>
            <CustomImage imageName={imageName} imageLink={imageLink} />
            <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
            <Typography align="center" variant="h6">
                Choose a new image
            </Typography>
            {/* <ImageUpload
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
            <Divider />
        </>
    );
};
