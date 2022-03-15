import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { Dialog, DialogTitle, DialogContent, Divider, Typography, responsiveFontSizes } from "@material-ui/core";
import React, { useCallback, useEffect, useState } from "react";
import { IPalavyrNode } from "@Palavyr-Types";
import { FileAssetDisplay } from "./CustomImage";
import { NodeFileAssetUpload } from "./FileAssetUpload";

export interface ImageNodeEditorProps {
    currentNode: IPalavyrNode;
    nodeId: string;
    fileAssetId?: string | null;
    repository: PalavyrRepository;
    editorIsOpen: boolean;
    closeEditor: () => void;
}

export const ImageNodeEditor = ({ currentNode, nodeId, repository, editorIsOpen, closeEditor, fileAssetId }: ImageNodeEditorProps) => {
    const [fileAssetLink, setFileAssetLink] = useState<string>("");
    const [fileAssetName, setFileAssetName] = useState<string>("");
    const [currentFileAssetId, setCurrentFileAssetId] = useState<string>("");

    const loadImage = useCallback(async () => {
        if (fileAssetId !== null && fileAssetId !== undefined) {
            const fileAssets = await repository.Configuration.FileAssets.GetFileAssets([fileAssetId]);
            const fileAsset = fileAssets[0];

            setFileAssetLink(fileAsset.link);
            setFileAssetName(fileAsset.fileName);
            setCurrentFileAssetId(fileAsset.fileId);
        }
    }, [currentFileAssetId]);

    useEffect(() => {
        loadImage();
    }, [currentFileAssetId]);

    return (
        <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
            <DialogTitle>Edit a conversation node</DialogTitle>
            <DialogContent>
                {currentFileAssetId && <FileAssetDisplay fileAssetId={currentFileAssetId} fileAssetName={fileAssetName} fileAssetLink={fileAssetLink} />}
                <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                <Typography align="center" variant="h6">
                    Choose a new image
                </Typography>
                <NodeFileAssetUpload
                    currentNode={currentNode}
                    nodeId={nodeId}
                    closeEditor={closeEditor}
                    currentImageId={currentFileAssetId}
                    setCurrentImageId={setCurrentFileAssetId}
                    setImageLink={setFileAssetLink}
                    setImageName={setFileAssetName}
                    repository={repository}
                    initialState={false}
                />
                <Divider />
            </DialogContent>
        </Dialog>
    );
};
