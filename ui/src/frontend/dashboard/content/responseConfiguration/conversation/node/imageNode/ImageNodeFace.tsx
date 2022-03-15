import { PalavyrRepository } from "@common/client/PalavyrRepository";
import React, { useCallback, useEffect, useState } from "react";
import { PalavyrNodeBody } from "../baseNode/PalavyrNodeBody";
import { FileAssetDisplay } from "./CustomImage";

export interface FileAssetNodeProps {
    openEditor: () => void;
    fileId?: string | null;
    repository: PalavyrRepository;
}

export const FileAssetNodeFace = ({ openEditor, fileId, repository }: FileAssetNodeProps) => {
    const [fileAssetLink, setFileAssetLink] = useState<string>("");
    const [fileAssetName, setFileAssetName] = useState<string>("");
    const [currrentFileAssetFileId, setCurrentFileId] = useState<string>("");

    const loadFileAsset = useCallback(async () => {
        if (fileId !== null && fileId !== undefined) {
            const fileAssets = await repository.Configuration.FileAssets.GetFileAssets([fileId]);
            const fileAsset = fileAssets[0];

            setFileAssetLink(fileAsset.link);
            setFileAssetName(fileAsset.fileName);
            setCurrentFileId(fileAsset.fileId);
        }
    }, [fileId]);

    useEffect(() => {
        loadFileAsset();
    }, [fileId]);

    return (
        <PalavyrNodeBody openEditor={openEditor} isFileAssetNode>
            <FileAssetDisplay fileAssetId={currrentFileAssetFileId} fileAssetName={fileAssetName} fileAssetLink={fileAssetLink} titleVariant="body1" />
        </PalavyrNodeBody>
    );
};
