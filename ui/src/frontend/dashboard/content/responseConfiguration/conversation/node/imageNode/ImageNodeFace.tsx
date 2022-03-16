import { PalavyrRepository } from "@common/client/PalavyrRepository";
import React, { useCallback, useEffect, useState } from "react";
import { PalavyrNodeBody } from "../baseNode/PalavyrNodeBody";
import { FileAssetDisplay } from "./FileAssetDisplay";

export interface FileAssetNodeProps {
    openEditor: () => void;
    fileAssetId?: string | null;
    repository: PalavyrRepository;
}

export const FileAssetNodeFace = ({ openEditor, fileAssetId, repository }: FileAssetNodeProps) => {
    const [fileAssetLink, setFileAssetLink] = useState<string>("");
    const [fileAssetName, setFileAssetName] = useState<string>("");
    const [currrentFileAssetFileId, setCurrentFileId] = useState<string>("");

    const loadFileAsset = useCallback(async () => {
        if (fileAssetId !== null && fileAssetId !== undefined) {
            const fileAssets = await repository.Configuration.FileAssets.GetFileAssets([fileAssetId]);
            const fileAsset = fileAssets[0];

            setFileAssetLink(fileAsset.link);
            setFileAssetName(fileAsset.fileName);
            setCurrentFileId(fileAsset.fileId);
        }
    }, [fileAssetId]);

    useEffect(() => {
        loadFileAsset();
    }, [fileAssetId]);

    return (
        <PalavyrNodeBody openEditor={openEditor} isFileAssetNode>
            <FileAssetDisplay fileAssetId={currrentFileAssetFileId} fileAssetName={fileAssetName} fileAssetLink={fileAssetLink} titleVariant="body1" />
        </PalavyrNodeBody>
    );
};
