import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { Dialog, DialogTitle, DialogContent, Divider, Typography, responsiveFontSizes } from "@material-ui/core";
import React, { useCallback, useEffect, useState } from "react";
import { IPalavyrNode } from "../../Contracts";
import { CustomImage } from "./CustomImage";
import { ImageUpload } from "./ImageUpload";

export interface ImageNodeEditorProps {
    currentNode: IPalavyrNode;
    nodeId: string;
    imageId?: string | null;
    repository: PalavyrRepository;
    editorIsOpen: boolean;
    closeEditor: () => void;
}

export const ImageNodeEditor = ({ currentNode, nodeId, repository, editorIsOpen, closeEditor, imageId }: ImageNodeEditorProps) => {
    const [imageLink, setImageLink] = useState<string>("");
    const [imageName, setImageName] = useState<string>("");
    const [currentImageId, setCurrentImageId] = useState<string>("");

    const loadImage = useCallback(async () => {
        if (imageId !== null && imageId !== undefined) {
            const fileLinks = await repository.Configuration.Images.getImages([imageId]);
            const fileLink = fileLinks[0];
            if (!fileLink.isUrl) {
                const presignedUrl = await repository.Configuration.Images.getSignedUrl(fileLink.s3Key, fileLink.fileId);
                setImageLink(presignedUrl);
                setImageName(fileLink.fileName);
                setCurrentImageId(fileLink.fileId);
            }
        }
    }, [currentImageId]);

    useEffect(() => {
        loadImage();
    }, [currentImageId]);
    return (
        <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
            <DialogTitle>Edit a conversation node</DialogTitle>
            <DialogContent>
                {currentImageId && <CustomImage imageId={currentImageId} imageName={imageName} imageLink={imageLink} />}
                <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                <Typography align="center" variant="h6">
                    Choose a new image
                </Typography>
                <ImageUpload
                    currentNode={currentNode}
                    nodeId={nodeId}
                    closeEditor={closeEditor}
                    currentImageId={currentImageId}
                    setCurrentImageId={setCurrentImageId}
                    setImageLink={setImageLink}
                    setImageName={setImageName}
                    repository={repository}
                    initialState={false}
                />
                <Divider />
            </DialogContent>
        </Dialog>
    );
};
