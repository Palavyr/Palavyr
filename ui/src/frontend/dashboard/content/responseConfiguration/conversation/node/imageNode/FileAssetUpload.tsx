import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { FileAssetResource, SetState } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext, useEffect } from "react";
import { useHistory } from "react-router-dom";
import { IPalavyrNode } from "@Palavyr-Types";
import { UploadOrSelectFromExisting } from "@common/uploads/UploadOrChooseFromExisting";

interface FileAssetUploadProps {
    currentNode: IPalavyrNode;
    closeEditor: () => void;
    setFileAsset: SetState<FileAssetResource>;
    repository: PalavyrRepository;
}

export const NodeFileAssetUpload = ({ setFileAsset, currentNode, closeEditor, repository }: FileAssetUploadProps) => {
    const history = useHistory();

    const { setSuccessOpen, setSuccessText, planTypeMeta } = useContext(DashboardContext);
    useEffect(() => {
        if (planTypeMeta && !planTypeMeta.allowedFileUpload) {
            history.push("/dashboard/please-subscribe");
        }
    }, [planTypeMeta]);

    const handleFileSave = async (files: File[]) => {
        if (files.length == 0) return;

        const formData = new FormData();
        files.forEach((file: File) => formData.append("files", file));

        const result = await repository.Configuration.FileAssets.UploadFileAssets(formData);
        setSuccessText("File Uploaded");

        currentNode.imageId = result[0].fileId;
        currentNode.UpdateTree();
        setSuccessOpen(true);
        closeEditor();
    };

    const onSelectChange = async (_: any, option: FileAssetResource) => {
        if (setFileAsset) {
            setFileAsset(option);
            currentNode.imageId = option.fileId;
            currentNode.UpdateTree();
            setSuccessOpen(true);
            closeEditor();
        }
    };

    return <>{currentNode && <UploadOrSelectFromExisting handleFileSave={handleFileSave} onSelectChange={onSelectChange} currentFileAssetId={currentNode.imageId} />}</>;
};
