import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { Dialog, DialogTitle, DialogContent, Divider, Typography } from "@material-ui/core";
import React, { useCallback, useEffect, useState } from "react";
import { FileAssetResource, IPalavyrNode } from "@Palavyr-Types";
import { FileAssetDisplay } from "./FileAssetDisplay";
import { NodeFileAssetUpload } from "./FileAssetUpload";

export interface ImageNodeEditorProps {
    currentNode: IPalavyrNode;
    fileAssetId?: string | null;
    repository: PalavyrRepository;
    editorIsOpen: boolean;
    closeEditor: () => void;
}

export const FileAssetNodeEditor = ({ currentNode, repository, editorIsOpen, closeEditor }: ImageNodeEditorProps) => {
    const [fileAsset, setFileAsset] = useState<FileAssetResource | null>(null);

    const loadImage = useCallback(async () => {
        if (currentNode.fileId) {
            const fileAssets = await repository.Configuration.FileAssets.GetFileAssets([currentNode.fileId]);
            const fileAsset = fileAssets[0];
            setFileAsset(fileAsset);
        }
    }, [currentNode.fileId]);

    useEffect(() => {
        loadImage();
    }, [currentNode.fileId, loadImage]);

    return (
        <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
            <DialogTitle>Edit a conversation node</DialogTitle>
            <DialogContent>
                {fileAsset && <FileAssetDisplay fileAsset={fileAsset} />}
                <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                <Typography align="center" variant="h6">
                    Choose a new image
                </Typography>
                <NodeFileAssetUpload setFileAsset={setFileAsset} currentNode={currentNode} closeEditor={closeEditor} repository={repository} />
                <Divider />
            </DialogContent>
        </Dialog>
    );
};
