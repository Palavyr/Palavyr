import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { FileAssetResource } from "@Palavyr-Types";
import React, { useCallback, useEffect, useState } from "react";
import { PalavyrNodeBody } from "../baseNode/PalavyrNodeBody";
import { FileAssetDisplay } from "./FileAssetDisplay";

export interface FileAssetNodeProps {
    openEditor: () => void;
    fileAssetId?: string | null;
    repository: PalavyrRepository;
}

export const FileAssetNodeFace = ({ openEditor, fileAssetId, repository }: FileAssetNodeProps) => {
    const [fileAsset, setFileAsset] = useState<FileAssetResource | null>(null);

    const loadFileAsset = useCallback(async () => {
        if (fileAssetId !== null && fileAssetId !== undefined) {
            const fileAssets = await repository.Configuration.FileAssets.GetFileAssets([fileAssetId]);
            const fileAsset = fileAssets[0];
            setFileAsset(fileAsset);
        }
    }, [fileAssetId]);

    useEffect(() => {
        loadFileAsset();
    }, [fileAssetId]);

    return (
        <PalavyrNodeBody openEditor={openEditor} isFileAssetNode>
            {fileAsset && <FileAssetDisplay fileAsset={fileAsset} titleVariant="body1" />}
        </PalavyrNodeBody>
    );
};
