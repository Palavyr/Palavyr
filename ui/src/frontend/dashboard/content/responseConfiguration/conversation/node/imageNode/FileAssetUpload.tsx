import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { Divider, Typography } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import { Upload } from "frontend/dashboard/content/responseConfiguration/uploadable/Upload";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import React, { useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { IPalavyrNode } from "@Palavyr-Types";
import { useNodeInterfaceStyles } from "../../nodeInterfaceStyles";
import { SelectFromExistingImages } from "./SelectFromExistingImages";
import { ACCEPTED_FILES } from "@constants";

interface FileAssetUploadProps {
    currentNode: IPalavyrNode;
    nodeId: string;
    closeEditor: () => void;
    currentFileAssetId: string;
    setCurrentFileAssetId: SetState<string>;
    setFileAssetLink: SetState<string>;
    setFileAssetName: SetState<string>;
    initialState: boolean;
    repository: PalavyrRepository;
}

export const NodeFileAssetUpload = ({ setCurrentFileAssetId, currentNode, nodeId, closeEditor, currentFileAssetId, setFileAssetLink, setFileAssetName, repository, initialState = false }: FileAssetUploadProps) => {
    const cls = useNodeInterfaceStyles({});
    const history = useHistory();
    const [uploadModal, setUploadModal] = useState(false);

    const { setSuccessOpen, setSuccessText, planTypeMeta } = useContext(DashboardContext);
    useEffect(() => {
        if (planTypeMeta && !planTypeMeta.allowedImageUpload) {
            history.push("/dashboard/please-subscribe");
        }
    }, [planTypeMeta]);

    const toggleModal = () => {
        setUploadModal(!uploadModal);
    };

    const handleFileSave = async (files: File[]) => {
        if (files.length == 0) return;

        const formData = new FormData();
        files.forEach((file: File) => formData.append("files", file));

        const result = await repository.Configuration.FileAssets.UploadFileAssets(formData);
        setSuccessText("Images Uploaded");

        await repository.Configuration.FileAssets.LinkFileAssetToNode(result[0].fileId, nodeId);
        setSuccessOpen(true);
        closeEditor();
    };

    return (
        <>
            {currentNode && (
                <>
                    <div className={cls.imageBlock}>
                        <SelectFromExistingImages
                            nodeId={nodeId}
                            repository={repository}
                            setFileAssetId={(imageId: string) => {
                                currentNode.imageId = imageId;
                                setCurrentFileAssetId(imageId);
                                currentNode.UpdateTree();
                            }}
                            setFileAssetLink={setFileAssetLink}
                            setFileAssetName={setFileAssetName}
                            currentFileAssetId={currentFileAssetId}
                        />
                    </div>
                    <Divider />
                    <div className={cls.imageBlock}>
                        {planTypeMeta && planTypeMeta.allowedImageUpload && (
                            <Upload
                                dropzoneType="area"
                                initialState={initialState}
                                modalState={uploadModal}
                                toggleModal={() => toggleModal()}
                                handleFileSave={handleFileSave}
                                summary="Upload a file."
                                buttonText="Upload"
                                uploadDetails={<Typography>Upload an image, pdf, or other document you wish to share with your users</Typography>}
                                acceptedFiles={ACCEPTED_FILES}
                            />
                        )}
                    </div>
                </>
            )}
        </>
    );
};
