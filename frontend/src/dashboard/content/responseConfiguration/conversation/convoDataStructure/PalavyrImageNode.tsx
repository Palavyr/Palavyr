import { Dialog, DialogTitle, DialogContent, Divider, Typography } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React, { useState, useCallback, useEffect } from "react";
import { CustomImage } from "../nodes/nodeInterface/nodeEditor/imageNode/CustomImage";
import { NodeImageUpload } from "../nodes/nodeInterface/nodeEditor/imageNode/ImageUpload";
import { NodeBody } from "./NodeBody";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { PalavyrNode } from "./PalavyrNode";

export class PalavyrImageNode extends PalavyrNode {
    constructor(containerList, repository, node, nodeList, rerender, leftMostBranch) {
        super(containerList, repository, node, nodeList, rerender, leftMostBranch);
    }

    renderNodeFace(setModalState: SetState<boolean>) {
        return () => {
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
                <NodeBody setModalState={setModalState}>
                    <CustomImage imageName={imageName} imageLink={imageLink} titleVariant="body1" />
                </NodeBody>
            );
        };
    }

    protected renderNodeEditor(modalState, setModalState) {
        return () => {
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

            const handleCloseModal = () => {
                setModalState(false);
            };

            return (
                <Dialog fullWidth open={modalState} onClose={handleCloseModal}>
                    <DialogTitle>Edit a conversation node</DialogTitle>
                    <DialogContent>
                        {this.imageId === null
                            ? this.renderImageEditorWhenEmpty(setModalState, currentImageId, setImageLink, setImageName)
                            : this.renderImageEditorWhenFull(setModalState, imageName, imageLink, currentImageId, setImageLink, setImageName)}
                    </DialogContent>
                </Dialog>
            );
        };
    }

    protected renderImageEditorWhenEmpty(setModalState, currentImageId, setImageLink, setImageName) {
        return () => {
            return (
                <>
                    <Typography align="center" variant="h6">
                        Upload an image
                    </Typography>
                    <NodeImageUpload setModalState={setModalState} nodeId={this.nodeId} currentImageId={currentImageId} setImageLink={setImageLink} setImageName={setImageName} />
                </>
            );
        };
    }

    protected renderImageEditorWhenFull(setModalState, imageName, imageLink, currentImageId, setImageLink, setImageName) {
        return () => {
            return (
                <>
                    <CustomImage imageName={imageName} imageLink={imageLink} />
                    <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                    <Typography align="center" variant="h6">
                        Choose a new image
                    </Typography>
                    <NodeImageUpload setModalState={setModalState} nodeId={this.nodeId} currentImageId={currentImageId} setImageLink={setImageLink} setImageName={setImageName} initialState={false} />
                    <Divider />
                </>
            );
        };
    }
}
